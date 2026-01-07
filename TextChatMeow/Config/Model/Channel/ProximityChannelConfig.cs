namespace TextChatMeow.Config.Model.Channel
{
    internal class ProximityChannelConfig
    {
        public bool Enabled { get; set; } = true;

        public string Id { get; set; } = "proximity";

        public string Name { get; set; } = "Proximity Channel";

        public float MaxDistance { get; set; } = 10f;
    }
}
