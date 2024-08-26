using PlayerRoles;
using Exiled.API.Features;
using TextChatMeow.Enum;

namespace TextChatMeow.Model
{
    internal class TeamChatMessage : AbstractChatMessage
    {
        public override ChatMessageType Type =>  ChatMessageType.TeamChat;

        private readonly Team _senderTeam;

        public override string Text { get; set; }

        public TeamChatMessage(string message, Player sender)
        {
            _senderTeam = sender.Role.Team;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            Text = message;
        }

        public override bool IsVisible(Player receiver)
        {
            if (receiver == null)
                return false;

            if (receiver.Role.Team == _senderTeam)
                return true;

            return false;
        }
    }
}
