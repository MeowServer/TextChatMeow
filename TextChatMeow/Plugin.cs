using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

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
// V2.0.0
//      Rewrite the code for better compabitility and extensibility.

namespace TextChatMeow
{
    internal class TextChatPlugin : Plugin<Config>
    {
        public static Plugin Instance { get; set; }

        // The name of the plugin
        public override string Name { get; } = "TextChatMeow";

        // The description of the plugin
        public override string Description { get; } = "";

        // The author of the plugin
        public override string Author { get; } = "MeowServerOwner";

        // The current version of the plugin
        public override Version Version { get; } = new Version(1, 0, 0, 0);

        // The required version of LabAPI (usually the version the plugin was built with)
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

        public override void Enable()
        {
        }

        public override void Disable()
        {
        }
    }
}
