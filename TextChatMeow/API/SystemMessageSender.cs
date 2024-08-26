using Exiled.API.Features;
using System;
using System.Collections.Generic;
using TextChatMeow.Model;
using UnityEngine;

namespace TextChatMeow.API
{
    public static class SystemMessageSender
    {
        [Obsolete]
        public static void SendMessage(string message, string source, List<Player> targets)
        {
            var ms = new SystemChatMessage(message, source, targets);
            MessagesList.AddMessage(ms);
        }

        [Obsolete]
        public static void SendMessage(string message, string source, List<Player> targets, Color color)
        {
            var ms = new SystemChatMessage(message, source, targets, color);
            MessagesList.AddMessage(ms);
        }

        public static void SendMessage(string message, IEnumerable<Player> receivers)
        {
            var ms = new SystemChatMessage(message, receivers);
            MessagesList.AddMessage(ms);
        }
    }
}
