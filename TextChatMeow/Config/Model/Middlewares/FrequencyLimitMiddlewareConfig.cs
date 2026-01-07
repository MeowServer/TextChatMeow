using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model.Middlewares
{
    internal class FrequencyLimitMiddlewareConfig
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 1;
        public double TimeIntervalSeconds { get; set; } = 5;
        public int MaxMessages { get; set; } = 5;
    }

}
