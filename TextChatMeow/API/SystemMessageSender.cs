using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextChatMeow.API
{
    public static class SystemMessageSender
    {
        public static void SendMessage(string message, string source, List<Player> targets)
        {
            var ms = new SystemChatMessage(message, source, targets);
            MessagesList.AddMessage(ms);
        }

        public static void SendMessage(string message, string source, List<Player> targets, Color color)
        {
            var ms = new SystemChatMessage(message, source, targets, color);
            MessagesList.AddMessage(ms);
        }
    }
}
