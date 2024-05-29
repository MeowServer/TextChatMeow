using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    internal static class MessagePool
    {
        private static List<ChatMessage> messageList = new List<ChatMessage>();

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
    }
}
