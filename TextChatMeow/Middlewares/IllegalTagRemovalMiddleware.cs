using System.Text.RegularExpressions;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Middlewares
{
    public class IllegalTagRemovalMiddleware : IMiddleware
    {
        public string Name => "IllegalTagRemovalMiddleware";

        public int Priority { get; } = 1;

        private static readonly Regex RichTextTagRegex = new Regex(
        @"<(?:\/?[a-zA-Z0-9]+(?:=[^>]*)?)>",
        RegexOptions.Compiled);

        public void Process(ChatContext chatContext)
        {
            if (string.IsNullOrEmpty(chatContext.Message.DisplayContent))
                return;

            chatContext.Message.DisplayContent = RichTextTagRegex.Replace(chatContext.Message.DisplayContent, string.Empty);

            return;
        }
    }
}
