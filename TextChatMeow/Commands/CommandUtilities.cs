using System.Text.RegularExpressions;

namespace TextChatMeow.Commands
{
    public static  class CommandUtilities
    {
        public static string ClearTags(string str)
        {
            return Regex.Replace(str, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase|RegexOptions.Compiled);
        }
    }
}
