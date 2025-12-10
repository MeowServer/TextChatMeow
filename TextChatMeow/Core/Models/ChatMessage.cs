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
        /// Gets the sender of the message
        /// </summary>
        public ReferenceHub Sender { get; }

        /// <summary>
        /// Gets the raw content of the message
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// Get or set the formatted content of the message for display purposes.
        /// Defaults to the same value as RawContent.
        /// </summary>
        public string DisplayContent { get; set; }

        /// <summary>
        /// Gets or sets a collection of key-value pairs that store additional metadata associated with the object.
        /// </summary>
        /// <remarks>The metadata dictionary can be used to attach arbitrary information to the object at
        /// runtime. Keys are case-sensitive and should be unique within the collection. This property is intended for
        /// extensibility and may be used by consumers to store custom data relevant to their application
        /// scenario.</remarks>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public ChatMessage(ReferenceHub sender, string rawContent)
        {
            Sender = sender;
            RawContent = rawContent;
            DisplayContent = rawContent; // Default to raw content
        }
    }
}
