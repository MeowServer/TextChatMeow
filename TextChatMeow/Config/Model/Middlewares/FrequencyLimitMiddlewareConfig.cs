namespace TextChatMeow.Config.Model.Middlewares
{
    internal class FrequencyLimitMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public double TimeIntervalSeconds { get; set; } = 5;
        public int MaxMessages { get; set; } = 5;
    }

}
