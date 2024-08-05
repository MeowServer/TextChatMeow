using Exiled.API.Features;
using HintServiceMeow;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace TextChatMeow
{
    internal class PlayerMessageHandler
    {
        private static Config Config => Plugin.instance.Config;

        private static readonly List<PlayerMessageHandler> MessagesManagers = new List<PlayerMessageHandler>();

        private static CoroutineHandle _autoUpdateCoroutine = Timing.RunCoroutine(AutoUpdateMethod());

        private readonly Player _player;

        private readonly DateTime _timeCreated = DateTime.Now;

        private readonly Hint _textChatTip = new Hint
        {
            YCoordinate = 700,
            Alignment = HintAlignment.Left,
        };

        private readonly List<Hint> _messageSlots = new List<Hint>()
        {
            new Hint
            {
                YCoordinate = 700,
                Alignment = HintAlignment.Left,
            },
            new Hint
            {
                YCoordinate = 720,
                Alignment = HintAlignment.Left,
            },
            new Hint
            {
                YCoordinate = 740,
                Alignment = HintAlignment.Left,
            }
        };

        public PlayerMessageHandler(VerifiedEventArgs ev)
        {
            var playerDisplay = PlayerDisplay.Get(ev.Player);
            this._player = Player.Get(playerDisplay.ReferenceHub);

            _textChatTip.Text = Config.ChatTip;

            //Add hint onto _player display
            playerDisplay.AddHint(_textChatTip);
            playerDisplay.AddHint(_messageSlots);

            MessagesManagers.Add(this);
        }

        public static void RemoveMessageManager(Player player)
        {
            MessagesManagers.RemoveAll(x => x._player == player);
        }

        public void UpdateMessage()
        {
            //Get all the message that should be display
            List<ChatMessage> displayableMessages = new List<ChatMessage>();

            try
            {
                displayableMessages = MessagesList
                    .messageList
                    .Where(x => x.CanSee(this._player))
                    .ToList();
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }

            //Update the message onto _player's screen
            try
            {
                foreach (Hint hint in _messageSlots)
                {
                    hint.Hide = true;
                }

                for (var i = 0; i < _messageSlots.Count() && i < displayableMessages.Count(); i++)
                {
                    ChatMessage message = displayableMessages[i];

                    string text = string.Empty;

                    if (Plugin.instance.Config.AddCountDown && Plugin.instance.Config.MessagesDisappears)
                    {
                        int countdown = Plugin.instance.Config.MessagesHideTime - (int)(DateTime.Now - message.TimeSent).TotalSeconds;
                        text += $"[{countdown}]";//Add countdown in front of the message (if enabled
                    }
                        

                    text += message.text;

                    text += new string(' ', message.TimeSent.Second % 10);

                    _messageSlots[i].Text = text;
                    _messageSlots[i].Hide = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            //Set tip's visibility based on the message's visibility
            try
            {
                TimeSpan tipTimeToDisplay = TimeSpan.FromSeconds(Plugin.instance.Config.TipDisplayTime);

                if (Plugin.instance.Config.TipDisappears == false||_messageSlots.Any(x => !x.Hide) || _timeCreated + tipTimeToDisplay >= DateTime.Now)
                {
                    _textChatTip.Hide = false;
                }
                else
                {
                    _textChatTip.Hide = true;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        private static IEnumerator<float> AutoUpdateMethod()
        {
            while (true)
            {
                try
                {
                    foreach (var manager in MessagesManagers)
                    {
                        manager.UpdateMessage();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}
