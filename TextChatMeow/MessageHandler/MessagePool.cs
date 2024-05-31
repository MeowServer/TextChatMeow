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
    internal static class MessagePool
    {
        private static List<ChatMessage> messageList = new List<ChatMessage>();

        private static CoroutineHandle CountdownCoroutine = Timing.RunCoroutine(CountdownMethod());
        public static void UpdateMessagePool(ItemRemovedEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool(ItemAddedEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool(ChangingRoleEventArgs ev) => UpdateMessagePool();

        public static void UpdateMessagePool()
        {
            if (!CountdownCoroutine.IsRunning)
                CountdownCoroutine = Timing.RunCoroutine(CountdownMethod());

            MessageManager.UpdateAllMessage();
        }

        public static List<ChatMessage> GetMessages() => messageList;

        public static void AddMessage(ChatMessage ms)
        {
            if (!CountdownCoroutine.IsRunning)
                CountdownCoroutine = Timing.RunCoroutine(CountdownMethod());

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

            if(CountdownCoroutine.IsRunning)
                Timing.KillCoroutines(CountdownCoroutine);
        }

        private static IEnumerator<float> CountdownMethod()
        {
            float timeInterval = 1f;

            while (true)
            {
                try
                {
                    messageList
                        .ForEach(x => x.CountDown -= (int)timeInterval);
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
