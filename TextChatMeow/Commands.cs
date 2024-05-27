using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using LogWritterMeow;
using PlayerRoles;
using PlayerRoles.Spectating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextChatMeow
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ProximityChat : ICommand
    {
        public string Command { get; } = "ProximityChat";

        public string[] Aliases { get; } = new[] { "c" };

        public string Description { get; } = "向周围的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!Plugin.instance.Config.AllowProximityChat)
            {
                response = "此频道已被禁用";
                return false;
            }

            if (player.IsMuted)
            {
                response = "您已被禁言，禁言期间无法使用文字交流";
                return false;
            }

            var str = string.Join(" ", arguments.ToArray());
            var message = new ProximityChatMessage(str, player);

            MessagePool.AddMessage(message);

            response = $"您的消息已被发送至周围玩家：{str}";
            return true;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class RadioChat : ICommand
    {
        public string Command { get; } = "RadioChat";

        public string[] Aliases { get; } = new[] { "rc" };

        public string Description { get; } = "通过无线电发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!Plugin.instance.Config.AllowRadioChat)
            {
                response = "此频道已被禁用";
                return false;
            }

            if (player.IsMuted)
            {
                response = "您已被禁言，禁言期间无法使用文字交流";
                return false;
            }

            if (!player.HasItem(ItemType.Radio))
            {
                response = "您没有对讲机，无法通过无线电发送消息";
                return false;
            }
            
            if(((Radio)player.Items.First(x=>x.Type == ItemType.Radio)).BatteryLevel <= 0)
            {
                response = "您的对讲机电量已经耗尽，无法通过无线电发送消息";
                return false;
            }

            ((Radio)player.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel--;

            var str = string.Join(" ", arguments.ToArray());
            var message = new RadioChatMessage(str, player);

            MessagePool.AddMessage(message);

            response = $"您的消息已通过无线电发送：{str}";
            return true;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class PublicChat : ICommand
    {
        public string Command { get; } = "PublicChat";

        public string[] Aliases { get; } = new[] { "pc" };

        public string Description { get; } = "向所有的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!Plugin.instance.Config.AllowPublicChat)
            {
                response = "此频道已被禁用";
                return false;
            }

            if (player.IsMuted)
            {
                response = "您已被禁言，禁言期间无法使用文字交流";
                return false;
            }

            var str = string.Join(" ", arguments.ToArray());
            var message = new PublicChatMessage(str, player);

            MessagePool.AddMessage(message);

            response = $"您的消息已被发送至所有玩家：{str}";
            return true;
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeamChat : ICommand
    {
        public string Command { get; } = "TeamChat";

        public string[] Aliases { get; } = new[] { "tc" };

        public string Description { get; } = "向同队伍的的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!Plugin.instance.Config.AllowTeamChat)
            {
                response = "此频道已被禁用";
                return false;
            }

            if (player.IsMuted)
            {
                response = "您已被禁言，禁言期间无法使用文字交流";
                return false;
            }

            var str = string.Join(" ", arguments.ToArray());
            var message = new TeamChatMessage(str, player);

            MessagePool.AddMessage(message);

            response = $"您的消息已被发送至同队伍的玩家：{str}";
            return true;
        }
    }
}
