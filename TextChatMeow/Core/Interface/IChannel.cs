using System.Collections.Generic;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    public interface IChannel
    {
        /// <summary>
        /// Gets the unique identifier for this instance. Should be lowercase.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of this channel.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of recipients who should receive the specified chat message.
        /// </summary>
        /// <param name="message">The chat context containing information about the message to be delivered. Cannot be null.</param>
        /// <returns>A list of <see cref="ReferenceHub"/> objects representing the recipients of the message. The list is empty
        /// if there are no recipients.</returns>
        public List<ReferenceHub> GetRecipients(ChatContext messageContext);

        /// <summary>
        /// Determines whether the specified chat message can access to this channel.
        /// </summary>
        /// <param name="message">The chat context containing information about the message to be delivered. Cannot be null.</param>
        /// <returns><c>true</c> if the message has access to this channel; otherwise, <c>false</c>.</returns>
        public bool HaveAccess(ChatContext messageContext, out string deniedReason);
    }
}
