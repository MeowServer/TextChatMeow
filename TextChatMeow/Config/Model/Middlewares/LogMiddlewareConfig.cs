using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model.Middlewares
{
    internal class LogMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = int.MaxValue;
        public string Format { get; set; } = "[ChatLog] {SenderDisplayedName}({SenderUserId}): {DisplayContent}(Raw Message: {RawContent})";
    }
}
