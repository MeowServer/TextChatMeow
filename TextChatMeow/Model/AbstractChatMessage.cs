using PlayerRoles;
using System;
using Exiled.API.Features;
using TextChatMeow.Enum;
using UnityEngine;

namespace TextChatMeow.Model
{
    internal abstract class AbstractChatMessage
    {
        public DateTime TimeSent { get; set; } = DateTime.Now;

        public abstract ChatMessageType Type { get; }

        public string SenderNickname { get; set; }
        public RoleTypeId SenderRoleType { get; set; }
        public Color SenderRoleColor { get; set; }

        public abstract string Text { get; set; }

        public override string ToString()
        {
            return $"TimeSent: {TimeSent} | Type: {Type} | SenderNickname: {SenderNickname} | SenderRoleType: {SenderRoleType}";
        }

        public abstract bool IsVisible(Player receiver);
    }


}
