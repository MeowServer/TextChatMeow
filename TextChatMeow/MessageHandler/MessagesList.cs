using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using TextChatMeow.Model;

namespace TextChatMeow.MessageHandler
{
    /// <summary>
    /// Contain all the messages that sent by the player
    /// Remove the message after time out
    /// </summary>
    internal static class MessagesList
    {
        private static readonly CoroutineHandle CountdownCoroutine = Timing.RunCoroutine(MessageListCoroutineMethod());

        public static readonly LinkedList<AbstractChatMessage> MessageList = new LinkedList<AbstractChatMessage>();

        private static readonly TimeSpan TimeOut = TimeSpan.FromSeconds(Plugin.Instance.Config.MessagesDisappearTime);

        public static void AddMessage(AbstractChatMessage ms)
        {
            MessageList.AddFirst(ms);

            LogWriterMeow.Logger.Info(ms.ToString(), "TextChatMeow");
        }

        public static void RemoveMessage(AbstractChatMessage ms)
        {
            MessageList.Remove(ms);
        }

        public static void ClearMessageList(RoundEndedEventArgs __)
        {
            MessageList.Clear();
        }

        public static void ClearMessageList()
        {
            MessageList.Clear();
        }

        private static IEnumerator<float> MessageListCoroutineMethod()
        {
            while (true)
            {
                try
                {
                    //Clear time out messages
                    if (Plugin.Instance.Config.MessagesDisappears)
                    {
                        if(DateTime.Now - MessageList?.Last?.Value?.TimeSent >= TimeOut)
                            MessageList?.RemoveLast();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error occured in MessageListCoroutineMethod");
                    Log.Error(e);
                }

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
