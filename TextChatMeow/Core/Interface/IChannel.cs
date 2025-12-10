using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    internal interface IChannel
    {
        /// <summary>
        /// Gets the unique identifier for this instance.
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
        public List<ReferenceHub> GetRecipients(ChatContext message);
    }
}
