using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    internal static class MessageList//I suspect someone accidentally use reflection to access this class
    {
        private static List<ChatMessage> messageList = new List<ChatMessage>();

        private static CoroutineHandle CountdownCoroutine = Timing.RunCoroutine(CountdownMethod());
        public static void UpdateMessagePool(ItemRemovedEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool(ItemAddedEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool(ChangingRoleEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool()
        {
            MessageManager.UpdateAllMessage();
        }

        public static List<ChatMessage> GetMessages() => messageList;

        public static void AddMessage(ChatMessage ms)
        {
            messageList.Insert(0, ms);
            MessageManager.UpdateAllMessage();

            LogWritterMeow.Logger.Info(ms.text);
        }

        public static void RemoveMessage(string message)
        {
            messageList.RemoveAll(x => x.text == message);
        }

        public static void ClearMessagePool(EndingRoundEventArgs ev) => ClearMessagePool();

        public static void ClearMessagePool()
        {
            messageList.Clear();
        }

        private static IEnumerator<float> CountdownMethod()
        {
            float timeInterval = 1f;

            while (true)
            {
                try
                {
                    if(Plugin.instance.Config.Debug && messageList.Count > 0)
                    {
                        string DebugInfo = string.Empty;
                        DebugInfo += "\n==============MessageList================\n";
                        DebugInfo += $"Message Count: {messageList.Count}\n";

                        foreach (var message in messageList)
                        {
                            DebugInfo += $"{message.ToString()}\n";
                        }

                        DebugInfo += "*********Removing Messages*********\n";

                        foreach(var message in MessageList.GetMessages())
                        {
                            if(DateTime.Now - message.TimeSent >= TimeSpan.FromSeconds(Plugin.instance.Config.MessagesHideTime))
                            {
                                DebugInfo += "Total Time Displayed: " + (DateTime.Now - message.TimeSent) + " | ";
                                DebugInfo += $"{message.ToString()}\n";
                            }
                        }

                        DebugInfo += "=========================================\n";

                        Log.Debug(DebugInfo);
                    }

                    if (messageList.Count > 0 && Plugin.instance.Config.MessagesDisappears)
                    {
                        messageList.RemoveAll(x =>
                        {
                            return DateTime.Now - x.TimeSent >= TimeSpan.FromSeconds(Plugin.instance.Config.MessagesHideTime);
                        });
                    }
                    
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                yield return Timing.WaitForSeconds(timeInterval);//if changes, also change the time in UpdateMessage
            }
        }
    }
}
