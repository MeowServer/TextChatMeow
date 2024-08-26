using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using TextChatMeow.Enum;
using UnityEngine;

namespace TextChatMeow.Model
{
    internal class SystemChatMessage : AbstractChatMessage
    {
        public override ChatMessageType Type => ChatMessageType.SystemsChat;

        private readonly List<Player> _receivers;

        public override string Text { get; set; }

        public SystemChatMessage(string message, IEnumerable<Player> receivers)
        {
            this.Text = message;

            _receivers = receivers.ToList();
        }

        [Obsolete]
        public SystemChatMessage(string message, string source, List<Player> targets)
        {
            this.Text = message;

            _receivers = new List<Player>(targets);
        }

        [Obsolete]
        public SystemChatMessage(string message, string source, List<Player> targets, Color color)
        {
            this.Text = message;

            _receivers = new List<Player>(targets);
        }

        public override bool IsVisible(Player receiver)
        {
            if (receiver == null) return false;
            if (_receivers.Contains(receiver)) return true;

            return false;
        }
    }
}
