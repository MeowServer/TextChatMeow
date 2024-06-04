using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextChatMeow.Commands
{
    public static  class MessageTools
    {
        public static string GetChannelName(ChatMessageType type)
        {
            return Plugin.instance.Config.ChannelsName[type];
        }

        public static string ClearTags(string str)
        {
            return Regex.Replace(str, @"<[^>]*>", string.Empty);
        }
    }
}
