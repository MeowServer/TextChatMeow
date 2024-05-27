using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow;
using MEC;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace TextChatMeow
{
    internal class TextChatMeow : Plugin<Config>
    {
        public static TextChatMeow instance { get; set; }

        public override string Name => "Text Chat Meow";
        public override string Author => "MeowServerOwner";
        public override Version Version => new Version(1, 1, 0);

        public override void OnEnabled()
        {
            HintServiceMeow.EventHandler.NewPlayer += CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left += DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound += MessageList.ClearMessageList;
            Exiled.Events.Handlers.Server.EndingRound += MessageList.ClearMessageList;

            Exiled.Events.Handlers.Player.ItemAdded += MessageList.UpdateMessageList;
            Exiled.Events.Handlers.Player.ItemRemoved += MessageList.UpdateMessageList;
            Exiled.Events.Handlers.Player.ChangingRole += MessageList.UpdateMessageList;

            base.OnEnabled();
            instance = this;
        }

        public override void OnDisabled()
        {
            HintServiceMeow.EventHandler.NewPlayer -= CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left -= DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound -= MessageList.ClearMessageList;
            Exiled.Events.Handlers.Server.EndingRound -= MessageList.ClearMessageList;

            Exiled.Events.Handlers.Player.ItemAdded -= MessageList.UpdateMessageList;
            Exiled.Events.Handlers.Player.ItemRemoved -= MessageList.UpdateMessageList;
            Exiled.Events.Handlers.Player.ChangingRole -= MessageList.UpdateMessageList;

            base.OnDisabled();
            instance = null;
        }

        public void CreateNewMessageManager(PlayerDisplay playerDisplay)
        {
            new MessageManager(playerDisplay);
        }

        public void DeleteMessageManager(LeftEventArgs ev)
        {
            MessageManager.RemoveMessageManager(ev.Player);
        }
    }

    internal static class MessageList
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

    internal class MessageManager
    {
        public static List<MessageManager> messagesManagers = new List<MessageManager>();

        private static CoroutineHandle AutoUpdateCoroutine = Timing.RunCoroutine(AutoUpdateMethod());

        private Player player;

        private TimeSpan timeCreated;

        private HintServiceMeow.Hint TextChatTip = new HintServiceMeow.Hint(580, HintAlignment.Left, TextChatMeow.instance.Config.ChatTip);
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

            timeCreated = Round.ElapsedTime;

            messagesManagers.Add(this);
        }

        public static void RemoveMessageManager(Player player)
        {
            messagesManagers.RemoveAll(x => x.player == player);
        }

        public void UpdateMessage()
        {
            int index = 0;

            foreach(var hint in MessageSlots)
            {
                hint.hide = true;
            }

            IEnumerable<string> displayableMessages = 
                from ChatMessage message in MessageList.messageList
                where message.CanSee(this.player)
                where message.TimeSent + TimeSpan.FromSeconds(10) > Round.ElapsedTime
                select message.text;

            foreach(string text in displayableMessages)
            {
                MessageSlots[index].message = text;
                MessageSlots[index].hide = false;

                index++;
                if(index >= MessageSlots.Count())
                {
                    break;
                }
            }

            if (MessageSlots.Any(x => !x.hide) || Round.ElapsedTime - timeCreated <= TimeSpan.FromSeconds(10))
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
            foreach(var chatMessage in messagesManagers)
            {
                chatMessage.UpdateMessage();
            }
        }

        private static IEnumerator<float> AutoUpdateMethod()
        {
            while(true)
            {
                foreach(var messageManager in messagesManagers)
                {
                    messageManager.UpdateMessage();
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
            MessageList.AddMessage(ms);
        }

        public static void SendMessage(string message, string source, List<Player> targets, Color color)
        {
            var ms = new SystemChatMessage(message, source, targets, color);
            MessageList.AddMessage(ms);
        }
    }
}
