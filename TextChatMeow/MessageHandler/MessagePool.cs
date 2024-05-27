using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    internal class MessagePool
    {
        public static List<ChatMessage> messageList = new List<ChatMessage>();

        public static void UpdateMessageList(ItemRemovedEventArgs ev)
        {
            MessageManager.UpdateAllMessage();
        }

        public static void UpdateMessageList(ItemAddedEventArgs ev)
        {
            MessageManager.UpdateAllMessage();
        }

        public static void UpdateMessageList(ChangingRoleEventArgs ev)
        {
            MessageManager.UpdateAllMessage();
        }

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

        public static void ClearMessageList(EndingRoundEventArgs ev)
        {
            messageList.Clear();
        }

        public static void ClearMessageList()
        {
            messageList.Clear();
        }
    }
}
