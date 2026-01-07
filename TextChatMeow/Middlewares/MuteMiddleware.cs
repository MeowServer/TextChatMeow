using LabApi.Features.Wrappers;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Middlewares
{
    public class MuteMiddleware : IMiddleware
    {
        public string Name => "MuteMiddleware";

        public int Priority { get; } = 1;

        public void Process(ChatContext chatContext)
        {
            // If sender is not a player, skip processing
            if (string.IsNullOrEmpty(chatContext.Message.SenderUserId))
                return;

            // Get the player
            var player = Player.Get(chatContext.Message.SenderUserId);
            if (player == null)
            {
                return; // Player not found, skip processing
            }

            // If muted, cancel the message
            if (player.IsMuted)
            {
                chatContext.Cancel("You are muted and cannot send messages.");
            }

            return;
        }
    }
}
