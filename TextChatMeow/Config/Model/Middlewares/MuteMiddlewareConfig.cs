namespace TextChatMeow.Config.Model.Middlewares
{
    internal class MuteMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public string MutedReason { get; set; } = "You are muted and cannot send messages.";
    }
}
