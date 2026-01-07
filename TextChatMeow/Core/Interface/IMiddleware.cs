using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    public interface IMiddleware
    {
        /// <summary>
        /// Gets the name associated with this middleware.
        /// </summary>
        public string Name { get; }

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
