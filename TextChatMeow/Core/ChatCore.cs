using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<IChatOutput> _outputs = new List<IChatOutput>();

        /// <summary>
        /// Registers the specified middleware component in the processing pipeline.
        /// </summary>
        /// <remarks>Middleware components are executed in order of their priority. Registering the same
        /// middleware instance or type more than once is not allowed.</remarks>
        /// <param name="middleware">The middleware instance to add. Must not already be registered.</param>
        /// <exception cref="InvalidOperationException">Thrown if a middleware of the same type is already registered.</exception>
        public void RegisterMiddleware(IMiddleware middleware)
        {
            if (_middlewares.Contains(middleware))
                throw new InvalidOperationException($"Middleware of type '{middleware.GetType().Name}' is already registered.");

            _middlewares.Add(middleware);

            _middlewares = _middlewares.OrderBy(m => m.Priority).ToList();
        }

        /// <summary>
        /// Unregisters a middleware component so that it is no longer invoked in the processing pipeline.
        /// </summary>
        /// <param name="middleware">The middleware instance to remove from the pipeline. Cannot be null.</param>
        public void UnregisterMiddleware(IMiddleware middleware)
        {
            _middlewares.Remove(middleware);
        }

        /// <summary>
        /// Unregisters all middleware components of the specified type from the middleware pipeline.
        /// </summary>
        /// <remarks>Use this method to remove all instances of a specific middleware type from the
        /// pipeline. This can be useful when dynamically modifying the middleware configuration at runtime.</remarks>
        /// <typeparam name="T">The type of middleware to remove. Must implement the IMiddleware interface.</typeparam>
        public void UnregisterMiddleware<T>() where T : IMiddleware
        {
            _middlewares.RemoveAll(m => m is T);
        }

        /// <summary>
        /// Registers the specified channel for use with the system.
        /// </summary>
        /// <param name="channel">The channel to register. Cannot be null. The channel's Id must be unique among all registered channels.</param>
        /// <exception cref="InvalidOperationException">Thrown if a channel with the same Id is already registered.</exception>
        public void RegisterChannel(IChannel channel)
        {
            if (_channels.ContainsKey(channel.Id))
                throw new InvalidOperationException($"Channel with id '{channel.Id}' is already registered.");

            _channels.Add(channel.Id, channel);
        }

        /// <summary>
        /// Unregisters the channel with the specified identifier, removing it from the collection of active channels.
        /// </summary>
        /// <param name="channelId">The unique identifier of the channel to unregister. Cannot be null.</param>
        public void UnregisterChannel(string channelId)
        {
            _channels.Remove(channelId);
        }

        /// <summary>
        /// Registers an output target to receive chat messages from this instance.
        /// </summary>
        /// <param name="output">The output target to add. Cannot be null. If the output is already registered, this method has no effect.</param>
        public void RegisterOutput(IChatOutput output)
        {
            if (_outputs.Contains(output))
                return;
            _outputs.Add(output);
        }

        /// <summary>
        /// Unregisters the specified chat output so that it no longer receives messages.
        /// </summary>
        /// <param name="output">The chat output instance to remove from the list of registered outputs. Cannot be null.</param>
        public void UnregisterOutput(IChatOutput output)
        {
            _outputs.Remove(output);
        }

        /// <summary>
        /// Unregisters all output handlers of the specified type from the chat system.
        /// </summary>
        /// <remarks>Use this method to remove all registered outputs of a given type. After calling this
        /// method, messages will no longer be sent to outputs of type T. This operation affects all instances of the
        /// specified type that have been registered.</remarks>
        /// <typeparam name="T">The type of output handler to unregister. Must implement the IChatOutput interface.</typeparam>
        public void UnregisterOutput<T>() where T : IChatOutput
        {
            _outputs.RemoveAll(o => o is T);
        }

        /// <summary>
        /// Attempts to send a chat message from the specified sender to the given channel. The message may be processed
        /// or cancelled by middleware before delivery.
        /// </summary>
        /// <remarks>If the specified channel does not exist or if any middleware cancels the message, the
        /// method returns false and the message is not delivered. Middleware may modify or cancel the message before it
        /// is sent to recipients.</remarks>
        /// <param name="channelId">The identifier of the channel to which the message should be sent. Must refer to a registered channel.</param>
        /// <param name="message">The content of the chat message to send.</param>
        /// <param name="senderNickname">The display name of the player, plugin, or server sending the message. </param>
        /// <param name="senderUserId">The unique identifier of the player sending the message. If this is sent by plugin or server, this value should be empty string.</param>
        /// <param name="cancelReason">If the message could not be sent, contains a description of the reason; otherwise, empty string.</param>
        /// <returns>true if the message was successfully delivered to the channel; otherwise, false.</returns>
        internal bool SendMessage(string channelId, string message, string senderNickname, string senderUserId, out string cancelReason)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message content cannot be null or empty.", nameof(message));

            if (string.IsNullOrEmpty(channelId))
                throw new ArgumentException("Channel ID cannot be null or empty.", nameof(channelId));

            if (!_channels.TryGetValue(channelId, out var channel))
                throw new InvalidOperationException($"Channel with id '{channelId}' is not registered.");

            var chatMessage = new ChatMessage(senderNickname, senderUserId, message);
            var chatContext = new ChatContext(channelId, chatMessage);

            // Process middlewares
            foreach (var middleware in _middlewares)
            {
                try
                {
                    middleware.Process(chatContext);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while processing chat middleware.", ex);
                }

                if (chatContext.IsCancelled)
                {
                    cancelReason = chatContext.CancelReason;
                    return false;
                }
            }

            // Check access to the channel
            try
            {
                if (!channel.HaveAccess(chatContext, out string deniedReason))
                {
                    cancelReason = deniedReason;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking channel access.", ex);
            }

            // Get recipients and send the message
            List<ReferenceHub> recipients;
            try
            {
                recipients = channel.GetRecipients(chatContext);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving chat message recipients.", ex);
            }

            // Send to outputs
            foreach (var displayOutput in _outputs)
            {
                try
                {
                    displayOutput.Send(recipients, chatContext);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while sending chat message to output.", ex);
                }
            }

            cancelReason = string.Empty;
            return true;
        }

        public bool SendMessage(string channelId, string message, out string cancelReason)
        {
            return SendMessage(channelId, message, "Server", string.Empty, out cancelReason);
        }

        public bool SendMessage(ReferenceHub sender, string channelId, string message, out string cancelReason)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            var player = Player.Get(sender);
            if (player == null)
            {
                throw new InvalidOperationException("Player not found for the given ReferenceHub.");
            }

            var nickname = player.Nickname;
            var userId = player.UserId;
            return SendMessage(channelId, message, nickname, userId, out cancelReason);
        }

        public bool SendMessage(string channelId, string message, string pluginName, out string cancelReason)
        {
            return SendMessage(channelId, message, pluginName, string.Empty, out cancelReason);
        }
    }
}
