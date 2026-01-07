using System;
using System.Collections.Generic;
using System.Linq;
using TextChatMeow.Config.Model;
using TextChatMeow.Config.Model.Channel;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Channels
{
    internal class PublicChannel : IChannel
    {
        public PublicChannelConfig Config => TextChatPlugin.Instance.Config.PublicChannel;
        public string Id => Config.Id;
        public string Name => Config.Name;

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
