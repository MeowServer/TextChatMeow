using Exiled.API.Features;
using MEC;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Utilities;
using TextChatMeow.Model;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;
using HintServiceMeow.Core.Enum;

namespace TextChatMeow.MessageHandler
{
    internal class DisplayManager
    {
        private static Config Config => Plugin.Instance.Config;
        
        private static readonly List<DisplayManager> MessagesManagers = [];
        private static CoroutineHandle _autoUpdateCoroutine;
        private static readonly Hint hint = new()
        {
            Text = Config.ChatTip,
            YCoordinate = Config.MessageYCoordinate,
            Alignment = Config.MessageAlignment,
        };
        private readonly Hint _textChatTip = hint;
        private readonly List<Hint> _messageSlots =
        [
            new() {
                YCoordinate = Config.MessageYCoordinate + 25,
                Alignment = Config.MessageAlignment,
                SyncSpeed = HintSyncSpeed.Fast
            },
            new() {
                YCoordinate = Config.MessageYCoordinate + 50,
                Alignment = Config.MessageAlignment,
                SyncSpeed = HintSyncSpeed.Fast
            },
            new() {
                YCoordinate = Config.MessageYCoordinate + 75,
                Alignment = Config.MessageAlignment,
                SyncSpeed = HintSyncSpeed.Fast
            }
        ];

        private readonly DateTime _timeCreated = DateTime.Now;
        private readonly TimeSpan _tipTimeToDisplay = TimeSpan.FromSeconds(Plugin.Instance.Config.TipDisappearTime);

        private readonly Player _player;

        public DisplayManager(VerifiedEventArgs ev)
        {
            _player = ev.Player;

            var playerDisplay = PlayerDisplay.Get(ev.Player);
            playerDisplay.AddHint(_textChatTip);
            playerDisplay.AddHint(_messageSlots);

            MessagesManagers.Add(this);

            if(!_autoUpdateCoroutine.IsRunning)
                _autoUpdateCoroutine = Timing.RunCoroutine(AutoUpdateMethod());
        }

        public static void RemoveMessageManager(Player player)
        {
            MessagesManagers.RemoveAll(x => x._player == player);
        }

        public void UpdateMessage()
        {
            try
            {
                IEnumerable<AbstractChatMessage>  displayableMessages = MessagesList.MessageList
                    .Where(x => x.IsVisible(this._player));

                _messageSlots.ForEach(hint => hint.Hide = true);

                using(var enumerator = displayableMessages.GetEnumerator())
                {
                    for (var i = 0; i < _messageSlots.Count && enumerator.MoveNext(); i++)
                    {
                        AbstractChatMessage message = enumerator.Current;

                        if(message == null)
                            continue;

                        int countDown = Plugin.Instance.Config.MessagesDisappearTime - (int)(DateTime.Now - message.TimeSent).TotalSeconds;

                        string text = Plugin.Instance.Config.ChatMessageTemplate
                            .Replace("{PlayerName}", message.SenderNickname)
                            .Replace("{CountDown}", countDown.ToString())
                            .Replace("{ChannelColor}", Plugin.Instance.Translation.ChannelsColor[message.Type].ToHex())
                            .Replace("{ChannelName}", Plugin.Instance.Translation.ChannelsName[message.Type])
                            .Replace("{RoleColor}", message.SenderRoleColor.ToHex())
                            .Replace("{RoleName}", Plugin.Instance.Translation.RoleName[message.SenderRoleType])
                            .Replace("{Message}", message.Text);

                        _messageSlots[i].Text = text;
                        _messageSlots[i].Hide = false;
                    }
                }

                if (Plugin.Instance.Config.TipDisappears == false||
                    _messageSlots.Any(x => !x.Hide) ||
                    _timeCreated + _tipTimeToDisplay >= DateTime.Now)
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

                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
