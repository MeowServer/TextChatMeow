namespace TextChatMeow.Config.Model
{
    internal class ChatCommandConfig
    {
        public string Command { get; set; } = "Chat";
        public string[] CommandAliases { get; set; } = new[] { "C", "c" };
        public string Description { get; set; } = "Send a chat message. Usage: .c <Message> or .c [@Channel] <Message>";
        public string DefaultChannelId { get; set; } = "public";
        public string ReponseWrongFormat { get; set; } = "Usage: .c [@ChannelName] <Message>";
        public string ResponseWrongChannelFormat { get; set; } = "Invalid channel format. Example: .c @public";
        public string ResponseNoMessage { get; set; } = "Please enter a message content.";
        public string ResponseUnknownError { get; set; } = "An unknown error occurred while sending the message.";
        public string ResponseSuccess { get; set; } = "Message sent successfully.";
        public string ResponseFailedToSend { get; set; } = "Failed to send message. Reason: ";
    }
}
