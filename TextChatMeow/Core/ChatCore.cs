using GameCore;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core
{
    public class ChatCore
    {
        public static ChatCore Instance { get; private set; } = new ChatCore();

        /// <summary>
        /// List of registered middlewares sorted by priority
        /// </summary>
        private List<IMiddleware> _middlewares = new List<IMiddleware>();

        /// <summary>
        /// Channels registered in the chat core
        /// </summary>
        private Dictionary<string, IChannel> _channels = new Dictionary<string, IChannel>();

        public void RegisterMiddleware(IMiddleware middleware)
        {
            if (_middlewares.Contains(middleware))
                return;

            _middlewares.Add(middleware);

            _middlewares = _middlewares.OrderBy(m => m.Priority).ToList();
        }

        public void UnregisterMiddleware(IMiddleware middleware)
        {
            _middlewares.Remove(middleware);
        }

        public void UnregisterMiddleware<T>() where T : IMiddleware
        {
            _middlewares.RemoveAll(m => m is T);
        }

        public bool SendMessage(ReferenceHub sender, string channelId, string message)
        {
            var player = Player.Get(sender);

            if (!_channels.TryGetValue(channelId, out var channel))
            {
                Logger.Warn($"[ChatCore] {player.Nickname}({player.UserId}) attempted to send message to unregistered channel '{channelId}'");
                return false;
            }

            var chatMessage = new ChatMessage(sender, message);
            var chatContext = new ChatContext(channelId, chatMessage);
            
            foreach (var middleware in _middlewares)
            {
                middleware.Process(chatContext);

                if (chatContext.IsCancelled)
                {
                    Logger.Warn($"[ChatCore] Message from {player.Nickname}({player.UserId}) to channel '{channelId}' was cancelled by middleware '{middleware.GetType().Name}' Reason: {chatContext.CancelReason}");
                    return false;
                }
            }

            List<ReferenceHub> recipients = channel.GetRecipients(chatContext);

            // TODO : Implement actual message sending logic here

            return true;
        }
    }
}
