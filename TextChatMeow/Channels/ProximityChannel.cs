using System;
using System.Collections.Generic;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using TextChatMeow.Config.Model;
using TextChatMeow.Config.Model.Channel;
using TextChatMeow.Core.Interface;
using TextChatMeow.Core.Models;

namespace TextChatMeow.Channels
{
    internal class ProximityChannel : IChannel
    {
        private ProximityChannelConfig Config => TextChatPlugin.Instance.Config.ProximityChannel;
        public string Id => Config.Id;
        public string Name => Config.Name;

        private float _maxDistance => Config.MaxDistance;

        public List<ReferenceHub> GetRecipients(ChatContext messageContext)
        {
            try
            {
                Player sender = Player.Get(messageContext.Message.SenderUserId);
                UnityEngine.Vector3 senderPosition = sender.Position;

                List<ReferenceHub> recipients = new List<ReferenceHub>();
                foreach (var player in Player.List)
                {
                    float distance = UnityEngine.Vector3.Distance(senderPosition, player.Position);
                    if (distance < _maxDistance)
                    {
                        recipients.Add(player.ReferenceHub);
                    }
                }

                return recipients;
            }
            catch (Exception ex)
            {
                var correlationId = Guid.NewGuid().ToString();
                Logger.Error($"ProximityChannel:GetRecipients failed (MessageId: {messageContext?.Message?.Guid}) - {ex}");
                return new List<ReferenceHub>();
            }
        }

        public bool HaveAccess(ChatContext messageContext, out string deniedReason)
        {
            throw new NotImplementedException();
        }
    }
}
