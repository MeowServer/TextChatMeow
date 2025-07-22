using CommandSystem;
using Exiled.API.Features;
using TextChatMeow.Model;
using TextChatMeow.MessageHandler;

namespace TextChatMeow.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PublicChat : ICommand
    {
        public string Command => "PublicChat";

        public string[] Aliases => ["pc"];

        public string Description => Plugin.Instance.Translation.PublicChatDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!CheckPermission(player, out response))
                return false;

            var message = string.Join(" ", [.. arguments]);
            SendMessage(message, player);

            response = Plugin.Instance.Translation.ResponseWhenSend.Replace("{Message}", message);
            return true;
        }

        public bool CheckPermission(Player player, out string response)
        {
            if (!Plugin.Instance.Config.AllowPublicChat)
            {
                response = Plugin.Instance.Translation.DisabledChatChannel;
                return false;
            }

            if (player.IsMuted)
            {
                response = Plugin.Instance.Translation.Muted;
                return false;
            }

            response = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            str = CommandUtilities.ClearTags(str);
            var message = new PublicChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }
}
