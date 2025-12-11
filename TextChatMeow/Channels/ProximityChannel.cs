using LabApi.Features.Console;
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
    public class ProximityChannel : IChannel
    {
        public string Id { get; } = "proximity";
        public string Name { get; } = "Proximity Channel";

        private float _maxDistance = 10f;

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
            catch(Exception ex)
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
