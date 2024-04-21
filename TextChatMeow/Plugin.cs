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

            LogWritterMeow.Logger.Info(ms.message);
        }

        public static void RemoveMessage(string message)
        {
            messageList.RemoveAll(x => x.message == message);
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

        private static CoroutineHandle AutoUpdateCoroutine;

        private Player player;
        private PlayerDisplay playerDisplay;

        private HintServiceMeow.Hint TextChatTip;
        private List<HintServiceMeow.Hint> MessageSlots;

        public MessageManager(PlayerDisplay playerDisplay)
        {
            this.player = playerDisplay.player;
            //Set up hint
            this.TextChatTip = new HintServiceMeow.Hint(600, HintAlignment.Left, TextChatMeow.instance.Config.ChatTip);
            MessageSlots = new List<HintServiceMeow.Hint>()
            {
                new HintServiceMeow.Hint(620, HintAlignment.Left, ""),
                new HintServiceMeow.Hint(640, HintAlignment.Left, ""),
                new HintServiceMeow.Hint(660, HintAlignment.Left, ""),
            };

            //Add hint onto player display
            playerDisplay.AddHint(TextChatTip);
            foreach(var hint in MessageSlots)
            {
                playerDisplay.AddHint(hint);
            }

            messagesManagers.Add(this);
        }

        public static void RemoveMessageManager(Player player)
        {
            messagesManagers.RemoveAll(x => x.player == player);
        }

        public void UpdateMessage()
        {
            int index = 0;

            foreach(ChatMessage chatMessage in MessageList.messageList)
            {
                if(chatMessage.CheckPermissionToSeeMessage(this.player))
                {
                    MessageSlots[index].message = chatMessage.message;

                    index++;
                    if(index >= MessageSlots.Count())
                    {
                        break;
                    }
                }
            }
            //clear rest of the slots
            for(;index < MessageSlots.Count(); index++)
            {
                MessageSlots[index].message = "";
            }
        }

        public static void UpdateAllMessage()
        {
            foreach(var chatMessage in messagesManagers)
            {
                chatMessage.UpdateMessage();
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
