
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using TextChatMeow.Config.Model;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Channels
{
    public class TeamChannel : IChannel
    {
        public TeamChannelConfig Config => TextChatPlugin.Instance.Config.TeamChannel;
        public string Id => Config.Id;
        public string Name => Config.Name;

        public List<ReferenceHub> GetRecipients(ChatContext messageContext)
        {
            try
            {
                var recipients = new List<ReferenceHub>();
                Player sender = Player.Get(messageContext.Message.SenderUserId);

                foreach (var hub in ReferenceHub.AllHubs)
                {
                    Player recipient = Player.Get(hub);
                    if (sender.Faction == recipient.Faction)
                    {
                        recipients.Add(hub);
                    }
                }

                return recipients;
            }
            catch (Exception ex)
            {
                Logger.Error($"ProximityChannel:GetRecipients failed (MessageId: {messageContext?.Message?.Guid}) - {ex}");
                return new List<ReferenceHub>();
            }
        }

        public bool HaveAccess(ChatContext message, out string deniedReason)
        {
            throw new System.NotImplementedException();
        }
    }
}
