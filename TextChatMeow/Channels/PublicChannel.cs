using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Channels
{
    internal class PublicChannel : IChannel
    {
        public string Id { get; } = "public";
        public string Name { get; } = "Public Channel";

        public List<ReferenceHub> GetRecipients(ChatContext message)
        {
            return ReferenceHub.AllHubs.ToList();
        }

        public bool HaveAccess(ChatContext message, out string deniedReason)
        {
            deniedReason = string.Empty;
            return true;
        }
    }
}
