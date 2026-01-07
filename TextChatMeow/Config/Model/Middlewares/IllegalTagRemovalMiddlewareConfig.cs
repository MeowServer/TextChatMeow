namespace TextChatMeow.Config.Model.Middlewares
{
    internal class IllegalTagRemovalMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public string TagRegexPattern { get; set; } = @"<(?:\/?[a-zA-Z0-9]+(?:=[^>]*)?)>";
    }
}
