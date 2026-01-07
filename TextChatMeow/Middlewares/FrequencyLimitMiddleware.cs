using System;
using System.Collections.Generic;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Middlewares
{
    public class FrequencyLimitMiddleware : IMiddleware
    {
        public string Name => "FrequencyLimitMiddleware";

        public int Priority { get; } = 1;

        private Dictionary<string, List<DateTime>> _userMessageTimestamps = new Dictionary<string, List<DateTime>>();
        private TimeSpan _timeInterval;
        private int _maxMessages;

        public FrequencyLimitMiddleware(TimeSpan timeInterval, int maxMessages)
        {
            _timeInterval = timeInterval;
            _maxMessages = maxMessages;
        }

        public void Process(ChatContext chatContext)
        {
            // Skip if sender is not a player
            if (chatContext.Message.SenderUserId == string.Empty)
            {
                return;
            }

            // Get or create message history for player
            if (!_userMessageTimestamps.TryGetValue(chatContext.Message.SenderUserId, out var timestamps))
            {
                timestamps = new List<DateTime>();
                _userMessageTimestamps[chatContext.Message.SenderUserId] = timestamps;
                return;
            }

            // Remove timestamps outside of time interval
            foreach (var timestamp in timestamps)
            {
                var now = DateTime.UtcNow;
                if (now - timestamp > _timeInterval)
                {
                    timestamps.Remove(timestamp);
                }
            }

            // Cancel if message frequency exceeded
            if (timestamps.Count >= _maxMessages)
            {
                chatContext.Cancel("Message frequency limit exceeded.");
                return;
            }

            // Add timestamp for current message and continue
            timestamps.Add(chatContext.Message.TimestampUtc);

            return;
        }
    }
}
