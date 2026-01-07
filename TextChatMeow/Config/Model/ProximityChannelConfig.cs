using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model
{
    internal class ProximityChannelConfig
    {
        public bool Enabled { get; set; } = true;

        public string Id { get; set; } = "proximity";

        public string Name { get; set; } = "Proximity Channel";

        public float MaxDistance { get; set; } = 10f;
    }
}
