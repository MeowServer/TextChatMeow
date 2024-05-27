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
        public TimeSpan TimeSent { get; set; }

        public abstract ChatMessageType Type { get; }

        protected string SenderNickname { get; set; }
        protected RoleTypeId SenderRoleType { get; set; }
        protected Color SenderRoleColor { get; set; }

        public abstract string text { get; set; }

        public ChatMessage()
        {
            TimeSent = Round.ElapsedTime;
        }

        public abstract bool CanSee(Player receiver);
    }

    internal class SystemChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; } = ChatMessageType.SystemsChat;

        private List<Player> receivers = new List<Player>();

        private string source;
        private Color sourceColor;

        private string _message;
        public override string text
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
            this.source = source;

            this.text = message;

            this.sourceColor = TextChatMeow.instance.Config.ChannelsColor[Type];

            receivers = new List<Player>(targets);
        }

        public SystemChatMessage(string message, string source, List<Player> targets, Color color)
        {
            this.source = source;

            this.text = message;

            receivers = new List<Player>(targets);

            this.sourceColor = color;
        }

        public override bool CanSee(Player receiver)
        {
            if (receiver == null) return false;
            if (receivers.Contains(receiver)) return true;

            return false;
        }
    }

    internal class ProximityChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; } = ChatMessageType.ProximityChat;

        private List<Player> receivers = new List<Player>();

        private string _message;
        public override string text
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

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.text = message;
        }

        public override bool CanSee(Player receiver)
        {
            if (receiver == null) return false;
            if (receivers.Contains(receiver)) return true;

            return false;
        }
    }

    internal class RadioChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; } = ChatMessageType.RadioChat;

        private string _message;
        public override string text
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
            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.text = message;
        }

        public override bool CanSee(Player receiver)
        {
            if (receiver == null) return false;
            if (!receiver.HasItem(ItemType.Radio)) return false;
            if (((Radio)receiver.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel <= 0) return false;

            return true;
        }
    }

    internal class PublicChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; } = ChatMessageType.PublicChat;

        private bool sendFromDead;
        private bool sendFromSCP;

        private string _message;
        public override string text
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
            sendFromDead = sender.IsDead;
            sendFromSCP = sender.IsScp;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.text = message;
        }

        public override bool CanSee(Player receiver)
        {
            if (receiver == null) return false;
            if (sendFromDead != receiver.IsDead && !TextChatMeow.instance.Config.AllowSpectatorsChatWithPublic) return false;
            if (sendFromSCP != receiver.IsScp && !TextChatMeow.instance.Config.SCPAndHumanPublicChat) return false;

            return true;
        }
    }

    internal class TeamChatMessage : ChatMessage
    {
        public override ChatMessageType Type { get; } = ChatMessageType.TeamChat;

        private Team senderTeam;

        private string _message;
        public override string text
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
            senderTeam = sender.Role.Team;

            SenderNickname = sender.Nickname;
            SenderRoleType = sender.Role.Type;
            SenderRoleColor = sender.Role.Color;

            this.text = message;
        }

        public override bool CanSee(Player receiver)
        {
            if (receiver == null) return false;
            if (receiver.Role.Team == senderTeam) return true;

            return false;
        }
    }
}
