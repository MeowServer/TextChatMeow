using Exiled.API.Features.Items;
using System.Linq;
using Exiled.API.Features;
using TextChatMeow.Enum;

namespace TextChatMeow.Model
{
    internal class RadioChatMessage : AbstractChatMessage
    {
        public override ChatMessageType Type => ChatMessageType.RadioChat;

        public override string Text { get; set; }

        public RadioChatMessage(string message, Player sender)
        {
            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            Text = message;
        }

        public override bool IsVisible(Player receiver)
        {
            if (receiver == null)
                return false;

            if (!receiver.HasItem(ItemType.Radio))
                return false;

            if (((Radio)receiver.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel <= 0)
                return false;

            return true;
        }
    }
}
