using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using TextChatMeow.Core;

namespace TextChatMeow.Command
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Chat : ICommand
    {
        public string Command => "Chat";

        public string[] Aliases { get; } = new[] { "C", "c" };

        public string Description => "Send a chat message. Usage: .c <Message> or .c [@Channel] <Message>";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Check if sender is a player
            if (arguments.Count == 0)
            {
                response = "Usage: .c [@ChannelName] <Message>";
                return false;
            }

            // Set default channel ID to "public" and extract message content
            string channelId = "public";
            IEnumerable<string> messageParts = arguments;

            // Extract channel ID if specified
            string firstArg = arguments.At(0);
            if (firstArg.StartsWith("@"))
            {
                if (firstArg.Length == 1)
                {
                    response = "Invalid channel format. Example: .c @world Hello";
                    return false;
                }

                channelId = firstArg.Substring(1);

                // Ensure there is message content after the channel ID
                if (arguments.Count < 2)
                {
                    response = "Please enter a message content.";
                    return false;
                }

                messageParts = messageParts.Skip(1);
            }

            // Normalize channel ID to lowercase
            channelId = channelId.ToLower();

            // Combine message parts into a single string
            string content = string.Join(" ", messageParts);

            // Send the message using ChatCore
            bool isSuccess = true;
            string cancelReason = string.Empty;
            var player = Player.Get(sender);
            try
            {
                isSuccess = ChatCore.Instance.SendMessage(player.ReferenceHub, channelId, content, out cancelReason);
            }
            catch (InvalidOperationException ex)
            {
                cancelReason = ex.Message;
                isSuccess = false;
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while executing Chat command: \n" + ex);

                cancelReason = "An unexpected error occurred while sending the message.";
                isSuccess = false;
            }

            if (isSuccess)
            {
                response = "Message sent.";
                return true;
            }
            else
            {
                response = $"Failed to send message. Reason: {cancelReason}";
                return false;
            }
        }
    }
}