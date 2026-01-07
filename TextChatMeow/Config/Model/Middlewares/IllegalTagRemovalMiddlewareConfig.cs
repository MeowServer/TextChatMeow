using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model.Middlewares
{
    internal class IllegalTagRemovalMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public string TagRegexPattern { get; set; } = @"<(?:\/?[a-zA-Z0-9]+(?:=[^>]*)?)>";
    }
}
