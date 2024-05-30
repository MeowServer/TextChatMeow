using Exiled.API.Features;
using HintServiceMeow;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextChatMeow
{
    internal class MessageManager
    {
        public static List<MessageManager> messagesManagers = new List<MessageManager>();

        private static CoroutineHandle AutoUpdateCoroutine = Timing.RunCoroutine(AutoUpdateMethod());

        private Player player;

        private DateTime timeCreated;

        private HintServiceMeow.Hint TextChatTip = new HintServiceMeow.Hint(580, HintAlignment.Left, Plugin.instance.Config.ChatTip);
        private List<HintServiceMeow.Hint> MessageSlots = new List<HintServiceMeow.Hint>()
            {
                new HintServiceMeow.Hint(600, HintAlignment.Left, ""),
                new HintServiceMeow.Hint(620, HintAlignment.Left, ""),
                new HintServiceMeow.Hint(640, HintAlignment.Left, ""),
            };

        public MessageManager(PlayerDisplay playerDisplay)
        {
            this.player = playerDisplay.player;

            //Add hint onto player display
            playerDisplay.AddHint(TextChatTip);
            playerDisplay.AddHints(MessageSlots);

            timeCreated = DateTime.Now;

            messagesManagers.Add(this);
        }

        public static void RemoveMessageManager(Player player)
        {
            messagesManagers.RemoveAll(x => x.player == player);
        }

        public void UpdateMessage()
        {
            //Get all the message that should be display
            List<ChatMessage> displayableMessages = new List<ChatMessage>();

            try
            {
                displayableMessages = MessagePool
                    .GetMessages()
                    .Where(x => x.CountDown > 0)
                    .Where(x => x.CanSee(this.player))
                    .ToList();
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }

            //Update the message onto player's screen
            try
            {
                foreach (HintServiceMeow.Hint hint in MessageSlots)
                {
                    hint.hide = true;
                }

                for (var i = 0; i < MessageSlots.Count() && i < displayableMessages.Count(); i++)
                {
                    ChatMessage message = displayableMessages[i];

                    MessageSlots[i].message = message.text;
                    MessageSlots[i].hide = false;
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

                if (MessageSlots.Any(x => !x.hide) || timeCreated + tipTimeToDisplay >= DateTime.Now)
                {
                    TextChatTip.hide = false;
                }
                else
                {
                    TextChatTip.hide = true;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static void UpdateAllMessage()
        {
            foreach (var manager in messagesManagers)
            {
                manager.UpdateMessage();
            }
        }

        private static IEnumerator<float> AutoUpdateMethod()
        {
            while (true)
            {
                try
                {
                    foreach (var messageManager in messagesManagers)
                    {
                        messageManager.UpdateMessage();
                    }

                    MessagePool
                        .GetMessages()
                        .Where(x => x.CountDown >= 0)
                        .ToList()
                        .ForEach(x => x.CountDown--);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                yield return Timing.WaitForSeconds(1f);//if changes, also change the time in UpdateMessage
            }
        }
    }

    public static class SystemMessageSender
    {
        public static void SendMessage(string message, string source, List<Player> targets)
        {
            var ms = new SystemChatMessage(message, source, targets);
            MessagePool.AddMessage(ms);
        }

        public static void SendMessage(string message, string source, List<Player> targets, Color color)
        {
            var ms = new SystemChatMessage(message, source, targets, color);
            MessagePool.AddMessage(ms);
        }
    }
}
