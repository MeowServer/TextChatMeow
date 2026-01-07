using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow.Config.Model
{
    public class TeamChannelConfig
    {
        public bool Enabled { get; set; } = true;
        public string Id { get; set; } = "team";
        public string Name { get; set; } = "Team Channel";
    }

}
