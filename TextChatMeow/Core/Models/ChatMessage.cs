using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets the date and time at which the message was created.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.Now;

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
            SenderDisplayedName = senderNickname;
            SenderUserId = senderUserId;
            RawContent = rawContent;
            DisplayContent = rawContent; // Default to raw content
        }
    }
}
