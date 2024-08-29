using Exiled.API.Interfaces;
using System.ComponentModel;
using HintServiceMeow.Core.Enum;

namespace TextChatMeow
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Translation for chat tip")]
        public string ChatTip { get; set; } = "输入.help查看聊天指令";

        [Description("Template for chat message")]
        public string ChatMessageTemplate { get; set; } = "{PlayerName}:[{CountDown}]<color={ChannelColor}>[{ChannelName}]</color><color={RoleColor}>[{RoleName}]</color>:{Message}";

        [Description("Position of message slots")]
        public HintAlignment MessageAlignment { get; set; } = HintAlignment.Left;
        public float MessageYCoordinate { get; set; } = 800;

        [Description("Should the tip disappear after a while?")]
        public bool TipDisappears { get; set; } = true;
        [Description("Should the message disappear after a while? Do not close this when useing CountDown tag in your template, otherwise error may occur.")]
        public bool MessagesDisappears { get; set; } = true;

        [Description("If use TipDisappears, how long should the tip display before it disappears?")]
        public int TipDisappearTime { get; set; } = 10;
        [Description("If use MessagesDisappears, how long should a message display before it disappears?")]
        public int MessagesDisappearTime { get; set; } = 10;

        [Description(
            "==============Proximity Chat==============\n" +
            "Allow proximity chat?")]
        public bool AllowProximityChat { get; set; } = true;
        [Description("How far should the message goes?")]
        public int ProximityChatDistance { get; set; } = 20;
        [Description("Allow chat between SCP and Human using proximity chat?")]
        public bool ScpAndHumanProximityChat { get; set; } = false;

        [Description(
            "==============Radio Chat==============\n" + 
            "Allow chat through radio?")]
        public bool AllowRadioChat { get; set; } = true;

        [Description("==============Team Chat==============\n" +
                     "Allow chat with teammate?")]
        public bool AllowTeamChat { get; set; } = false;

        [Description("==============Public Chat==============\n" +
            "Allow chat with everyone?")]
        public bool AllowPublicChat { get; set; } = false;
        [Description("Allow spectators chat with alives using public chat?")]
        public bool AllowSpectatorsChat { get; set; } = false;
        [Description("Allow chat between SCP and Human using public chat?")]
        public bool AllowScpAndHumanChat { get; set; } = false;
    }
}
