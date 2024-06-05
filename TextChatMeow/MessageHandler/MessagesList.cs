using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    /// <summary>
    /// Contain all of the messages that sent by the player
    /// Remove the message after the time out
    /// </summary>
    internal static class MessagesList
    {
        private static List<ChatMessage> _messageList = new List<ChatMessage>();

        public static ReadOnlyCollection<ChatMessage> messageList
        {
            get
            {
                return _messageList.AsReadOnly();
            }
        }

        private static CoroutineHandle CountdownCoroutine = Timing.RunCoroutine(MessageListCoroutineMethod());

        public static void AddMessage(ChatMessage ms)
        {
            Log.Debug("Add Message : " + ms.ToString());
            _messageList.Insert(0, ms);

            try
            {
                LogWritterMeow.Logger.Info(ms.text);
            }
            catch(Exception ex) 
            {
                
            }
        }

        public static void RemoveMessage(ChatMessage ms)
        {
            Log.Debug("Remove Message : " + ms.ToString());
            _messageList.Remove(ms);
        }

        public static void ClearMessageList(RoundEndedEventArgs ev)
        {
            Log.Debug("Clearing Message List Since Round Ended");
            _messageList.Clear();
        }

        public static void ClearMessageList()
        {
            Log.Debug("Clearing Message List Since Round Restarted");
            _messageList.Clear();
        }

        private static IEnumerator<float> MessageListCoroutineMethod()
        {
            float timeInterval = 1f;

            while (true)
            {
                try
                {
                    //Debug Info
                    if(Plugin.instance != null && Plugin.instance.Config.Debug && messageList.Count > 0)
                    {
                        string DebugInfo = string.Empty;
                        DebugInfo += "\n==============MessageList================\n";
                        DebugInfo += $"Message Count: {messageList.Count}\n";

                        foreach (var message in messageList)
                        {
                            DebugInfo += $"{message.ToString()}\n";
                        }

                        DebugInfo += "*********Removing Messages*********\n";

                        foreach(var message in messageList)
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

                    //Clear time out messages
                    if (messageList.Count > 0 && Plugin.instance.Config.MessagesDisappears)
                    {
                        _messageList?.RemoveAll(x => DateTime.Now - x.TimeSent >= TimeSpan.FromSeconds(Plugin.instance.Config.MessagesHideTime) );
                    }
                    
                }
                catch (Exception e)
                {
                    Log.Error("Error occured in MessageListCoroutineMethod");
                    Log.Error(e);
                }

                yield return Timing.WaitForSeconds(timeInterval);//if changes, also change the time in UpdateMessage
            }
        }
    }
}
