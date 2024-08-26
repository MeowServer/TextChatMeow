using PlayerRoles;
using System.Collections.Generic;
using Exiled.API.Features;
using TextChatMeow.Enum;
using UnityEngine;

namespace TextChatMeow.Model
{
    internal class ProximityChatMessage : AbstractChatMessage
    {
        public override ChatMessageType Type => ChatMessageType.ProximityChat;

        private readonly List<Player> _receivers = new List<Player>();

        public override string Text { get; set; }

        public ProximityChatMessage(string message, Player sender)
        {
            foreach (var player in Player.List)
            {

                if (sender.Role.Team == Team.SCPs && player.Role.Team == Team.SCPs)
                {
                    _receivers.Add(player);
                    continue;
                }

                if (!Plugin.Instance.Config.ScpAndHumanProximityChat)
                {
                    if (sender.Role.Team == Team.SCPs && player.Role.Team != Team.SCPs)
                        continue;

                    if (player.Role.Team == Team.SCPs)
                        continue;
                }

                var distance = Vector3.Distance(player.Position, sender.Position);
                if (distance <= Plugin.Instance.Config.ProximityChatDistance)
                {
                    _receivers.Add(player);
                }
            }

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.Text = message;
        }

        public override bool IsVisible(Player receiver)
        {
            if (receiver == null)
                return false;

            if (_receivers.Contains(receiver))
                return true;

            return false;
        }
    }
}
