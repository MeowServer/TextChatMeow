using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace TextChatMeow.Core.Models
{
    /// <summary>
    /// Represent a chat message sent by the player
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Gets the unique identifier associated with this message.
        /// </summary>
        public Guid Guid { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the display name of the sender. This value could be the nickname of the player, name of plugin, or the server.
        /// </summary>
        public string SenderDisplayedName { get; }

        /// <summary>
        /// Gets the unique identifier of the sender. This value will be the user ID of the player, or empty if the sender is not a player.
        /// </summary>
        public string SenderUserId { get; }

        /// <summary>
        /// Gets the raw content of the message.
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// Get or set the formatted content of the message for display purposes.
        /// Defaults to the same value as RawContent.
        /// </summary>
        public string DisplayContent { get; set; }

        /// <summary>
        /// Gets the UTC timestamp indicating when the message was created.
        /// </summary>
        public DateTime TimestampUtc { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a collection of key-value pairs that store additional metadata associated with the object.
        /// </summary>
        /// <remarks>The metadata dictionary can be used to attach arbitrary information to the object at
        /// runtime. Keys are case-sensitive and should be unique within the collection. This property is intended for
        /// extensibility and may be used by consumers to store custom data relevant to their application
        /// scenario.</remarks>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public ChatMessage(string senderNickname, string senderUserId, string rawContent)
        {
            if (senderNickname is null)
                throw new ArgumentNullException(nameof(senderNickname));

            if (senderUserId is null)
                throw new ArgumentNullException(nameof(senderUserId));

            if (rawContent is null)
                throw new ArgumentNullException(nameof(rawContent));

            SenderDisplayedName = senderNickname;
            SenderUserId = senderUserId;
            RawContent = rawContent;
            DisplayContent = rawContent; // Default to raw content
        }

        public ChatMessage(ReferenceHub sender, string rawContent)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            if (rawContent is null)
                throw new ArgumentNullException(nameof(rawContent));

            var player = Player.Get(sender);
            SenderDisplayedName = player.Nickname;
            SenderUserId = player.UserId;
            RawContent = rawContent;
            DisplayContent = rawContent; // Default to raw content
        }
    }
}
