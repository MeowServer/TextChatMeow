using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    public interface IMiddleware
    {
        /// <summary>
        /// Gets the priority level associated with the current instance. The lower the value, the higher the priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Processes the specified chat context to perform the required chat operations.
        /// </summary>
        /// <param name="chatContext">The chat context to process. Cannot be null.</param>
        public void Process(ChatContext chatContext);
    }
}
