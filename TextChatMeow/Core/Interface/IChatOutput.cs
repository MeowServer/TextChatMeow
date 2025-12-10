using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    public interface IChatOutput
    {
        void Send(List<ReferenceHub> hubs, ChatContext chatContext);
    }
}
