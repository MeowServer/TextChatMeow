using LabApi.Features.Console;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Middlewares
{
    public class LogMiddleware : IMiddleware
    {
        public string Name => "LogMiddleware";

        public int Priority { get; } = int.MaxValue;
        public void Process(ChatContext chatContext)
        {
            Logger.Info($"[ChatLog] {chatContext.Message.SenderDisplayedName}({chatContext.Message.SenderUserId}): {chatContext.Message.RawContent}(Raw Message: {chatContext.Message.RawContent})");
        }
    }
}
