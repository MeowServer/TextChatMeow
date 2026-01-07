using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using TextChatMeow.Config.Model;
using TextChatMeow.Core;

namespace TextChatMeow.Command
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Chat : ICommand
    {
        private ChatCommandConfig Config => TextChatPlugin.Instance.Config.Command;

        public string Command => Config.Command;

        public string[] Aliases => Config.CommandAliases;

        public string Description => Config.Description;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Check if sender is a player
            if (arguments.Count == 0)
            {
                response = Config.ReponseWrongFormat;
                return false;
            }

            // Set default channel ID to "public" and extract message content
            string channelId = Config.DefaultChannelId;
            IEnumerable<string> messageParts = arguments;

            // Extract channel ID if specified
            string firstArg = arguments.At(0);
            if (firstArg.StartsWith("@"))
            {
                if (firstArg.Length == 1)
                {
                    response = Config.ResponseWrongChannelFormat;
                    return false;
                }

                channelId = firstArg.Substring(1);

                // Ensure there is message content after the channel ID
                if (arguments.Count < 2)
                {
                    response = Config.ResponseNoMessage;
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

                cancelReason = Config.ResponseUnknownError;
                isSuccess = false;
            }

            if (isSuccess)
            {
                response = Config.ResponseSuccess;
                return true;
            }
            else
            {
                response = Config.ResponseFailedToSend + cancelReason;
                return false;
            }
        }
    }
}