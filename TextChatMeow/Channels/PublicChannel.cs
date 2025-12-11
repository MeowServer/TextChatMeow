using System;
using System.Collections.Generic;
using System.Linq;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Channels
{
    public class PublicChannel : IChannel
    {
        public string Id { get; } = "public";
        public string Name { get; } = "Public Channel";

        public List<ReferenceHub> GetRecipients(ChatContext message)
        {
            return ReferenceHub.AllHubs.ToList();
        }

        public bool HaveAccess(ChatContext message, out string deniedReason)
        {
            throw new NotImplementedException();
        }
    }
}
