using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Core.Models
{
    /// <summary>
    /// Represent the context of a chat message being processed. Includes the message itself
    /// </summary>
    public class ChatContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for the channel the message is assigned to.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the chat message associated with the current context.
        /// </summary>
        public ChatMessage Message { get; set; }

        /// <summary>
        /// Gets a value indicating whether the operation has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; } = false;

        /// <summary>
        /// Gets the reason provided for canceling the operation.
        /// </summary>
        public string CancelReason { get; private set; }

        internal ChatContext(string channelId, ChatMessage message)
        {
            if(channelId is null)
                throw new ArgumentNullException(nameof(channelId));

            if(message is null)
                throw new ArgumentNullException(nameof(message));

            ChannelId = channelId;
            Message = message;
        }

        /// <summary>
        /// Cancels the current operation and records the specified reason for cancellation.
        /// </summary>
        /// <remarks>After calling this method, the operation is marked as cancelled and the provided
        /// reason is stored. Subsequent calls to this method will overwrite the previous cancellation reason.</remarks>
        /// <param name="reason">The explanation for why the operation is being cancelled. This value is recorded for reference and may be
        /// displayed to users or logged. Cannot be null.</param>
        public void Cancel(string reason)
        {
            if(reason is null)
                throw new ArgumentNullException(nameof(reason));

            IsCancelled = true;
            CancelReason = reason;
        }
    }
}
