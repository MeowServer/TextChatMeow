using Exiled.API.Features;
using TextChatMeow.Enum;

namespace TextChatMeow.Model
{
    internal class PublicChatMessage : AbstractChatMessage
    {
        public override ChatMessageType Type => ChatMessageType.PublicChat;

        private readonly bool _sendFromDead;
        private readonly bool _sendFromSCP;

        public override string Text { get; set; }

        public PublicChatMessage(string message, Player sender)
        {
            _sendFromDead = sender.IsDead;
            _sendFromSCP = sender.IsScp;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.Text = message;
        }

        public override bool IsVisible(Player receiver)
        {
            if (receiver == null)
                return false;

            if (_sendFromDead != receiver.IsDead && !Plugin.Instance.Config.AllowSpectatorsChat)
                return false;

            if (_sendFromSCP != receiver.IsScp && !Plugin.Instance.Config.AllowScpAndHumanChat)
                return false;

            return true;
        }
    }
}
