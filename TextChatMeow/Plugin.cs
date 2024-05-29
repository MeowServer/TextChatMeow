using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow;
using MEC;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

//  V1.2.0
//      fixing bugs
//  V1.2.1
//      fixing bugs

namespace TextChatMeow
{
    internal class Plugin : Plugin<Config>
    {
        public static Plugin instance { get; set; }

        public override string Name => "TextChatMeow";
        public override string Author => "MeowServerOwner";
        public override Version Version => new Version(1, 2, 1);

        public override void OnEnabled()
        {
            HintServiceMeow.EventHandler.NewPlayer += EventHandler.CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left += EventHandler.DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound += MessagePool.ClearMessageList;
            Exiled.Events.Handlers.Server.EndingRound += MessagePool.ClearMessageList;

            Exiled.Events.Handlers.Player.ItemAdded += MessagePool.UpdateMessageList;
            Exiled.Events.Handlers.Player.ItemRemoved += MessagePool.UpdateMessageList;
            Exiled.Events.Handlers.Player.ChangingRole += MessagePool.UpdateMessageList;

            base.OnEnabled();
            instance = this;
        }

        public override void OnDisabled()
        {
            HintServiceMeow.EventHandler.NewPlayer -= EventHandler.CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left -= EventHandler.DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound -= MessagePool.ClearMessageList;
            Exiled.Events.Handlers.Server.EndingRound -= MessagePool.ClearMessageList;

            Exiled.Events.Handlers.Player.ItemAdded -= MessagePool.UpdateMessageList;
            Exiled.Events.Handlers.Player.ItemRemoved -= MessagePool.UpdateMessageList;
            Exiled.Events.Handlers.Player.ChangingRole -= MessagePool.UpdateMessageList;

            base.OnDisabled();
            instance = null;
        }
    }

    public static class EventHandler
    {
        public static void CreateNewMessageManager(PlayerDisplay playerDisplay)
        {
            new MessageManager(playerDisplay);
        }

        public static void DeleteMessageManager(LeftEventArgs ev)
        {
            MessageManager.RemoveMessageManager(ev.Player);
        }
    }
}
