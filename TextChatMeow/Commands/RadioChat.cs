using CommandSystem;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using TextChatMeow.Model;
using TextChatMeow.MessageHandler;

namespace TextChatMeow.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RadioChat : ICommand
    {
        public string Command => "RadioChat";

        public string[] Aliases => ["rc"];

        public string Description => Plugin.Instance.Translation.RadioChatDescription;

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
            if (!Plugin.Instance.Config.AllowRadioChat)
            {
                response = Plugin.Instance.Translation.DisabledChatChannel;
                return false;
            }

            if (player.IsMuted)
            {
                response = Plugin.Instance.Translation.Muted;
                return false;
            }

            if (!player.HasItem(ItemType.Radio))
            {
                response = Plugin.Instance.Translation.NoRadio;
                return false;
            }

            //If all radio is disabled, return false
            if (!player.Items.Where(x => x.Type == ItemType.Radio).Any(x => ((Radio)x).IsEnabled))
            {
                response = Plugin.Instance.Translation.RadioDied;
                return false;
            }

            //If all radio is out of battery, return false
            if (!player.Items.Where(x => x.Type == ItemType.Radio).Any(x => ((Radio)x).BatteryLevel > 0))
            {
                response = Plugin.Instance.Translation.RadioDied;
                return false;
            }

            response = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            ((Radio)player.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel--;

            str = CommandUtilities.ClearTags(str);
            var message = new RadioChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }
}
