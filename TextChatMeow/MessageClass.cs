using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextChatMeow
{
    public enum ChatMessageType
    {
        ProximityChat,
        RadioChat,
        PublicChat,
        TeamChat,
        SystemsChat,
    }

    internal abstract class ChatMessage
    {
        public abstract ChatMessageType Type { get; set; }

        protected abstract string SenderNickname { get; set; }
        protected abstract RoleTypeId SenderRoleType { get; set; }
        protected abstract Color SenderRoleColor { get; set; }

        public abstract string message { get; set; }

        public abstract bool CheckPermissionToSeeMessage(Player receiver);
    }

    internal class SystemChatMessage : ChatMessage
    {
        private List<Player> receivers = new List<Player>();

        public override ChatMessageType Type { get; set; }

        protected override string SenderNickname { get; set; }//Not Used
        protected override RoleTypeId SenderRoleType { get; set; }//Not Used
        protected override Color SenderRoleColor { get; set; }//Not Used

        private string source;
        private Color sourceColor;


        private string _message;
        public override string message
        {
            get
            {
                var str = "<b><color={colorOfChannel}>[{Channel}]</color><color={colorOfRole}>[{role}]</color></b>"
                    .Replace("{colorOfChannel}", TextChatMeow.instance.Config.ChannelsColor[Type].ToHex())
                    .Replace("{Channel}", TextChatMeow.instance.Config.ChannelsName[Type])
                    .Replace("{role}", source)
                    .Replace("{colorOfRole}", sourceColor.ToHex());

                str += _message;

                return str;
            }
            set
            {
                _message = value;
            }
        }

        public SystemChatMessage(string message, string source, List<Player> targets)
        {
            Type = ChatMessageType.SystemsChat;

            this.source = source;

            this.message = message;

            this.sourceColor = TextChatMeow.instance.Config.ChannelsColor[Type];

            receivers = new List<Player>(targets);
        }

        public SystemChatMessage(string message, string source, List<Player> targets, Color color)
        {
            Type = ChatMessageType.SystemsChat;

            this.source = source;

            this.message = message;

            receivers = new List<Player>(targets);

            this.sourceColor = color;
        }

        public override bool CheckPermissionToSeeMessage(Player receiver)
        {
            if (receiver == null) return false;
            if (receivers.Contains(receiver)) return true;

            return false;
        }
    }

    internal class ProximityChatMessage : ChatMessage
    {
        private List<Player> receivers = new List<Player>();

        public override ChatMessageType Type { get; set; }

        protected override string SenderNickname { get; set; }
        protected override RoleTypeId SenderRoleType { get; set; }
        protected override Color SenderRoleColor { get; set; }

        private string _message;
        public override string message
        {
            get
            {
                var str = "{PlayerNickName}: <b><color={colorOfChannel}>[{Channel}]</color><color={colorOfRole}>[{role}]</color></b>"
                    .Replace("{PlayerNickName}", SenderNickname)
                    .Replace("{colorOfChannel}", TextChatMeow.instance.Config.ChannelsColor[Type].ToHex())
                    .Replace("{Channel}", TextChatMeow.instance.Config.ChannelsName[Type])
                    .Replace("{role}", TextChatMeow.instance.Config.RoleName[SenderRoleType])
                    .Replace("{colorOfRole}", SenderRoleColor.ToHex());

                str += _message;

                return str;
            }
            set
            {
                _message = value;
            }
        }


        public ProximityChatMessage(string message, Player sender)
        {
            foreach (var player in Player.List)
            {

                if (sender.Role.Team == Team.SCPs && player.Role.Team == Team.SCPs)
                {
                    receivers.Add(player);
                    continue;
                }

                if (!TextChatMeow.instance.Config.SCPAndHumanProximityChat)
                {
                    if (sender.Role.Team == Team.SCPs && player.Role.Team != Team.SCPs)
                    {
                        continue;
                    }
                    else
                    {
                        if (player.Role.Team == Team.SCPs)
                        {
                            continue;
                        }
                    }
                }

                var distance = Vector3.Distance(player.Position, sender.Position);
                if (distance <= TextChatMeow.instance.Config.ProximityChatDistance)
                {
                    receivers.Add(player);
                }

            }

            Type = ChatMessageType.ProximityChat;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.message = message;
        }

        public override bool CheckPermissionToSeeMessage(Player receiver)
        {
            if (receiver == null) return false;
            if (receivers.Contains(receiver)) return true;

            return false;
        }
    }

    internal class RadioChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; set; }

        protected override string SenderNickname { get; set; }
        protected override RoleTypeId SenderRoleType { get; set; }
        protected override Color SenderRoleColor { get; set; }

        private string _message;
        public override string message
        {
            get
            {
                var str = "{PlayerNickName}: <b><color={colorOfChannel}>[{Channel}]</color><color={colorOfRole}>[{role}]</color></b>"
                    .Replace("{PlayerNickName}", SenderNickname)
                    .Replace("{colorOfChannel}", TextChatMeow.instance.Config.ChannelsColor[Type].ToHex())
                    .Replace("{Channel}", TextChatMeow.instance.Config.ChannelsName[Type])
                    .Replace("{role}", TextChatMeow.instance.Config.RoleName[SenderRoleType])
                    .Replace("{colorOfRole}", SenderRoleColor.ToHex());

                str += _message;

                return str;
            }
            set
            {
                _message = value;
            }
        }

        public RadioChatMessage(string message, Player sender)
        {
            Type = ChatMessageType.RadioChat;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.message = message;
        }

        public override bool CheckPermissionToSeeMessage(Player receiver)
        {
            if (receiver == null) return false;
            if (!receiver.HasItem(ItemType.Radio)) return false;
            if (((Radio)receiver.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel <= 0) return false;

            return true;
        }
    }

    internal class PublicChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; set; }

        private bool sendFromDead;
        private bool sendFromSCP;

        protected override string SenderNickname { get; set; }
        protected override RoleTypeId SenderRoleType { get; set; }
        protected override Color SenderRoleColor { get; set; }

        private string _message;
        public override string message
        {
            get
            {
                var str = "{PlayerNickName}: <b><color={colorOfChannel}>[{Channel}]</color><color={colorOfRole}>[{role}]</color></b>"
                    .Replace("{PlayerNickName}", SenderNickname)
                    .Replace("{colorOfChannel}", TextChatMeow.instance.Config.ChannelsColor[Type].ToHex())
                    .Replace("{Channel}", TextChatMeow.instance.Config.ChannelsName[Type])
                    .Replace("{role}", TextChatMeow.instance.Config.RoleName[SenderRoleType])
                    .Replace("{colorOfRole}", SenderRoleColor.ToHex());

                str += _message;

                return str;
            }
            set
            {
                _message = value;
            }
        }

        public PublicChatMessage(string message, Player sender)
        {
            Type = ChatMessageType.PublicChat;

            sendFromDead = sender.IsDead;
            sendFromSCP = sender.IsScp;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.message = message;
        }

        public override bool CheckPermissionToSeeMessage(Player receiver)
        {
            if (receiver == null) return false;
            if (sendFromDead != receiver.IsDead && !TextChatMeow.instance.Config.AllowSpectatorsChatWithPublic) return false;
            if (sendFromSCP != receiver.IsScp && !TextChatMeow.instance.Config.SCPAndHumanPublicChat) return false;

            return true;
        }
    }

    internal class TeamChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; set; }

        private Team senderTeam;

        protected override string SenderNickname { get; set; }
        protected override RoleTypeId SenderRoleType { get; set; }
        protected override Color SenderRoleColor { get; set; }

        private string _message;
        public override string message
        {
            get
            {
                var str = "{PlayerNickName}: <b><color={colorOfChannel}>[{Channel}]</color><color={colorOfRole}>[{role}]</color></b>"
                    .Replace("{PlayerNickName}", SenderNickname)
                    .Replace("{colorOfChannel}", TextChatMeow.instance.Config.ChannelsColor[Type].ToHex())
                    .Replace("{Channel}", TextChatMeow.instance.Config.ChannelsName[Type])
                    .Replace("{role}", TextChatMeow.instance.Config.RoleName[SenderRoleType])
                    .Replace("{colorOfRole}", SenderRoleColor.ToHex());

                str += _message;

                return str;
            }
            set
            {
                _message = value;
            }
        }

        public TeamChatMessage(string message, Player sender)
        {
            Type = ChatMessageType.TeamChat;

            senderTeam = sender.Role.Team;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.message = message;
        }

        public override bool CheckPermissionToSeeMessage(Player receiver)
        {
            if (receiver == null) return false;
            if (receiver.Role.Team == senderTeam) return true;

            return false;
        }
    }
}
