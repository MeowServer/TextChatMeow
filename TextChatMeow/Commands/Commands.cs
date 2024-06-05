using UnityEngine;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TextChatMeow.Commands;

namespace TextChatMeow
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ProximityChat : ICommand, IChatCommand
    {
        public string Command { get; } = "ProximityChat";

        public string[] Aliases { get; } = new[] { "c" };

        public string Description { get; } = "向周围的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!CheckPermission(player, out response))
                return false;

            var str = string.Join(" ", arguments.ToArray());
            SendMessage(str, player);

            response = $"您的消息已被发送至周围玩家：{str}";
            return true;
        }

        public bool CheckPermission(Player player, out string resposne)
        {
            if (!Plugin.instance.Config.AllowProximityChat)
            {
                resposne = "此频道已被禁用";
                return false;
            }

            if (player.IsMuted)
            {
                resposne = "您已被禁言，禁言期间无法使用文字交流";
                return false;
            }

            resposne = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            str = CommandTools.ClearTags(str);
            var message = new ProximityChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class RadioChat : ICommand, IChatCommand
    {
        public string Command { get; } = "RadioChat";

        public string[] Aliases { get; } = new[] { "rc" };

        public string Description { get; } = "通过无线电发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if(!CheckPermission(player, out response))
                return false;

            var str = string.Join(" ", arguments.ToArray());
            SendMessage(str, player);

            response = $"您的消息已通过无线电发送：{str}";
            return true;
        }

        public bool CheckPermission(Player player, out string response)
        {
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

            if (((Radio)player.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel <= 0)
            {
                response = "您的对讲机电量已经耗尽，无法通过无线电发送消息";
                return false;
            }

            response = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            ((Radio)player.Items.First(x => x.Type == ItemType.Radio)).BatteryLevel--;

            str = CommandTools.ClearTags(str);
            var message = new RadioChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class PublicChat : ICommand, IChatCommand
    {
        public string Command { get; } = "PublicChat";

        public string[] Aliases { get; } = new[] { "pc" };

        public string Description { get; } = "向所有的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!CheckPermission(player, out response))
                return false;

            var str = string.Join(" ", arguments.ToArray());
            SendMessage(str, player);

            response = $"您的消息已被发送至所有玩家：{str}";
            return true;
        }

        public bool CheckPermission(Player player, out string response)
        {
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

            response = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            str = CommandTools.ClearTags(str);
            var message = new PublicChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeamChat : ICommand, IChatCommand
    {
        public string Command { get; } = "TeamChat";

        public string[] Aliases { get; } = new[] { "tc" };

        public string Description { get; } = "向同队伍的的玩家发送一条消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (!CheckPermission(player, out response))
                return false;

            var str = string.Join(" ", arguments.ToArray());
            SendMessage(str, player);

            response = $"您的消息已被发送至同队伍的玩家：{str}";
            return true;
        }

        public bool CheckPermission(Player player, out string response)
        {

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

            response = string.Empty;
            return true;
        }

        public void SendMessage(string str, Player player)
        {
            str = CommandTools.ClearTags(str);
            var message = new TeamChatMessage(str, player);

            MessagesList.AddMessage(message);
        }
    }
}
