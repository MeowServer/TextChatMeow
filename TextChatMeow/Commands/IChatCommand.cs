using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextChatMeow
{
    public interface IChatCommand
    {
        bool CheckPermission(Player player, out string response);
    }
}
