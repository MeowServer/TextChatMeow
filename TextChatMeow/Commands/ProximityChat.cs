using CommandSystem;
using Exiled.API.Features;
using TextChatMeow.Model;
using TextChatMeow.MessageHandler;

namespace TextChatMeow.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ProximityChat : ICommand
    {
        public string Command => "ProximityChat";

        public string[] Aliases => ["c"];

        public string Description => Plugin.Instance.Translation.ProximityChatDescription;

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
            if (!Plugin.Instance.Config.AllowProximityChat)
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
            var message = new ProximityChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }
}
