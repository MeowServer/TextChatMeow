using Exiled.API.Extensions;
using Exiled.API.Interfaces;
using PlayerRoles;

using Exiled.API.Features.Roles;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HintServiceMeow.Core.Enum;
using UnityEngine;

namespace TextChatMeow
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public HintAlignment MessageAlignment { get; set; } = HintAlignment.Left;
        public float MessageYCoordinate { get; set; } = 800;

        [Description("Should the tip disappear after a while?")]
        public bool TipDisappears { get; set; } = true;
        [Description("Should the message disappear after a while? Do not close this when CountDownTip is active, otherwise error may occur.")]
        public bool MessagesDisappears { get; set; } = true;
        [Description("Should the tip have a countdown in front of them? This will only be active when MessagesDisappears is active.")]
        public bool CountDownTip { get; set; } = false;

        [Description("How long should the tip display before it disappears?")]
        public int TipDisappearTime { get; set; } = 10;
        [Description("How long should a message display before it disappears?")]
        public int MessagesDisappearTime { get; set; } = 10;

        [Description("Proximity Chat Config\nAllow proximity chat?")]
        public bool AllowProximityChat { get; set; } = true;
        [Description("How far should the message goes?")]
        public int ProximityChatDistance { get; set; } = 20;
        [Description("Allow chat between SCP and Human using proximity chat?")]
        public bool ScpAndHumanProximityChat { get; set; } = false;

        [Description("Allow chat through radio?")]
        public bool AllowRadioChat { get; set; } = true;

        [Description("Allow chat within team?")]
        public bool AllowTeamChat { get; set; } = false;

        [Description("Public Chat Config\nAllow chat with everyone?")]
        public bool AllowPublicChat { get; set; } = false;
        [Description("Allow spectators chat with alives using public chat?")]
        public bool AllowSpectatorsChatWithPublic { get; set; } = false;
        [Description("Allow chat between SCP and Human using public chat?")]
        public bool ScpAndHumanPublicChat { get; set; } = false;

        [Description("Translation for chat tip")]
        public string ChatTip { get; set; } = "输入.help查看聊天指令";

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
