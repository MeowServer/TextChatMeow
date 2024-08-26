using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;
using TextChatMeow.Enum;
using UnityEngine;

namespace TextChatMeow
{
    internal class Translation:ITranslation
    {
        public string ResponseWhenSend { get; set; } = "Successfully send message: {Message}";
        public string DisabledChatChannel { get; set; } = "This channel has been disabled";
        public string Muted { get; set; } = "You cannot send any message since you are muted";

        public string NoRadio { get; set; } = "You don't have a radio to send message";
        public string RadioDied { get; set; } = "Your radio has run out of battery and cannot send messages";

        public string ProximityChatDescription { get; set; } = "Send a chat message to proximate players";
        public string PublicChatDescription { get; set; } = "Send a chat message to public";
        public string RadioChatDescription { get; set; } = "Send a chat message through radio";
        public string TeamChatDescription { get; set; } = "Send a chat message to teammate";

        [Description("Translation of different channels")]
        public Dictionary<ChatMessageType, string> ChannelsName { get; set; } = new Dictionary<ChatMessageType, string> {
            {ChatMessageType.ProximityChat, "临近对话" },
            {ChatMessageType.RadioChat, "无线电频道" },
            {ChatMessageType.PublicChat, "公共频道" },
            {ChatMessageType.TeamChat, "团队频道" },
            {ChatMessageType.SystemsChat, "系统通知" }
        };

        [Description("Colors for different channels, set to black to let message's color equals to sender's team color")]
        public Dictionary<ChatMessageType, Color> ChannelsColor { get; set; } = new Dictionary<ChatMessageType, Color> {
            {ChatMessageType.ProximityChat, Color.grey },
            {ChatMessageType.RadioChat, Color.cyan },
            {ChatMessageType.PublicChat, Color.white },
            {ChatMessageType.TeamChat, Color.black },
            {ChatMessageType.SystemsChat, Color.red },
        };

        [Description("Translation of different role types")]
        public Dictionary<RoleTypeId, string> RoleName { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            { RoleTypeId.None, "无角色"},
            { RoleTypeId.Scp173, "Scp173"},
            { RoleTypeId.ClassD, "D级人员"},
            { RoleTypeId.Spectator, "观察者"},
            { RoleTypeId.Scp106, "Scp106"},
            { RoleTypeId.NtfSpecialist, "九尾狐收容专家"},
            { RoleTypeId.Scp049, "Scp049"},
            { RoleTypeId.Scientist, "科学家"},
            { RoleTypeId.Scp079, "Scp079"},
            { RoleTypeId.ChaosConscript, "混沌征召兵"},
            { RoleTypeId.Scp096, "Scp096"},
            { RoleTypeId.Scp0492, "Scp049-2"},
            { RoleTypeId.NtfSergeant, "九尾狐中士"},
            { RoleTypeId.NtfCaptain, "九尾狐队长"},
            { RoleTypeId.NtfPrivate, "九尾狐列兵"},
            { RoleTypeId.Tutorial, "教程人员"},
            { RoleTypeId.FacilityGuard, "设施警卫"},
            { RoleTypeId.Scp939, "Scp939"},
            { RoleTypeId.CustomRole, "自定义角色"},
            { RoleTypeId.ChaosRifleman, "混沌步枪兵"},
            { RoleTypeId.ChaosMarauder, "混沌掠夺者"},
            { RoleTypeId.ChaosRepressor, "混沌压制者"},
            { RoleTypeId.Overwatch, "角色Overwatch"},
            { RoleTypeId.Filmmaker, "摄像机"},
            { RoleTypeId.Scp3114, "Scp3114"},
        };
    }
}
