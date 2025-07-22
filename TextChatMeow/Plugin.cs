using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using TextChatMeow.MessageHandler;

//  V1.2.0
//      fixing bugs
//  V1.2.1
//      fixing bugs
// V1.2.2
//      Use count down instead of calculating the time directly. Bug fixing
// V1.2.3
//      Bug fixing
// V1.2.4
//      Bug fixing
// V1.2.5
//      Use DateTime instead of count down. Bug fixing
// V1.3.0
//      Use regex to clear rich-Text tags.
// V1.3.1
//      Bug fixing, fixed the bug that the message will be cleared every 2 seconds
// V1.4.0
//      Rewrite for HintServiceMeow V5.0.0
// V1.4.1
//      Add translation support
//      Improve code quality
// V1.4.2
//      Fix the bug that the message template did not include sender's nickname
// V1.4.3
//      Make it work with a newer version of log writer.

namespace TextChatMeow
{
    internal class Plugin : Plugin<Config, Translation>
    {
        public static Plugin Instance { get; set; } = new();

        public override string Name => "TextChatMeow";
        public override string Author => "MeowServerOwner";
        public override Version Version => new (1, 4, 2);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Verified += EventHandler.CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left += EventHandler.DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound += MessagesList.ClearMessageList;
            Exiled.Events.Handlers.Server.RoundEnded += MessagesList.ClearMessageList;

            base.OnEnabled();
            Instance = this;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= EventHandler.CreateNewMessageManager;
            Exiled.Events.Handlers.Player.Left -= EventHandler.DeleteMessageManager;

            Exiled.Events.Handlers.Server.RestartingRound -= MessagesList.ClearMessageList;
            Exiled.Events.Handlers.Server.RoundEnded -= MessagesList.ClearMessageList;

            base.OnDisabled();
#pragma warning disable CS8625 //null。
            Instance = null;
#pragma warning restore CS8625 //null。
        }
    }

    public static class EventHandler
    {
        public static void CreateNewMessageManager(VerifiedEventArgs ev)
        {
            _ = new DisplayManager(ev);
        }

        public static void DeleteMessageManager(LeftEventArgs ev)
        {
            DisplayManager.RemoveMessageManager(ev.Player);
        }
    }
}
