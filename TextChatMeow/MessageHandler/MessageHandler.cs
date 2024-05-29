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
            TimeSpan messageTimeToDisplay = TimeSpan.FromSeconds(Plugin.instance.Config.MessagesHideTime);

            Log.Info(messageTimeToDisplay.ToString());
            Log.Info($"CurrentTime: {DateTime.Now}");

            foreach (ChatMessage message in MessagePool.GetMessages())
            {
                Log.Info($"Message : {message.Type} Time: {message.TimeSent}");
                Log.Info($"Should be hide :{message.TimeSent + messageTimeToDisplay >= DateTime.Now}");
            }

            IEnumerable<string> displayableMessages =
                from ChatMessage message in MessagePool.GetMessages()
                where message.TimeSent + messageTimeToDisplay <= DateTime.Now
                where message.CanSee(this.player)
                select message.text;

            //Update the message onto player's screen
            foreach (var hint in MessageSlots)
            {
                hint.hide = true;
            }

            int index = 0;

            foreach (string text in displayableMessages)
            {
                MessageSlots[index].message = text;
                MessageSlots[index].hide = false;

                index++;
                if (index >= MessageSlots.Count())
                {
                    break;
                }
            }

            //Set tip's visibility based on the message's visibility
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

                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                yield return Timing.WaitForSeconds(1f);
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
