using TextChatMeow.Channels;
using TextChatMeow.Config.Model;
using TextChatMeow.Config.Model.Channel;
using TextChatMeow.Config.Model.Middlewares;
using TextChatMeow.Middlewares;

namespace TextChatMeow.Config
{
    internal class Config
    {
        // Command
        public ChatCommandConfig Command { get; set; } = new ChatCommandConfig();

        // Channels
        public ProximityChannelConfig ProximityChannel { get; set; } = new ProximityChannelConfig();
        public PublicChannelConfig PublicChannel { get; set; } = new PublicChannelConfig();
        public TeamChannelConfig TeamChannel { get; set; } = new TeamChannelConfig();

        // Middlewares
        public FrequencyLimitMiddlewareConfig FrequencyLimitMiddleware { get; set; } = new FrequencyLimitMiddlewareConfig();
        public IllegalTagRemovalMiddlewareConfig IllegalTagRemovalMiddleware { get; set; } = new IllegalTagRemovalMiddlewareConfig();
        public LogMiddlewareConfig LogMiddleware { get; set; } = new LogMiddlewareConfig();
        public MuteMiddlewareConfig MuteMiddleware { get; set; } = new MuteMiddlewareConfig();

    }
}
