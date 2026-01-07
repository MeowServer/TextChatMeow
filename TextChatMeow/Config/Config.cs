using TextChatMeow.Config.Model;

namespace TextChatMeow.Config
{
    internal class Config
    {
        public ChatCommandConfig Command { get; set; } = new ChatCommandConfig();
        public ProximityChannelConfig ProximityChannel { get; set; } = new ProximityChannelConfig();
        public PublicChannelConfig PublicChannel { get; set; } = new PublicChannelConfig();
        public TeamChannelConfig TeamChannel { get; set; } = new TeamChannelConfig();
    }
}
