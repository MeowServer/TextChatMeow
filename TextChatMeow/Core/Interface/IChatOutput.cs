using System.Collections.Generic;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Core.Interface
{
    public interface IChatOutput
    {
        void Send(List<ReferenceHub> hubs, ChatContext chatContext);
    }
}
