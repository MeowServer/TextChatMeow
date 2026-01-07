using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model.Middlewares
{
    internal class MuteMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public string MutedReason { get; set; } = "You are muted and cannot send messages.";
    }
}
