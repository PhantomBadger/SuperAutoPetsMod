using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Logging.API;
using Settings;
using SuperAutoPetsMod;
using SuperAutoPetsMod.MonoBehaviours;
using SuperAutoPetsMod.Patching;
using SuperAutoPetsMod.Twitch;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Unity;

namespace SuperAutoPetsMod
{
    [BepInPlugin("org.phantombadger.plugins.superautopetsmod", "Super Auto Pets Mod", "0.0.0.1")]
    [BepInProcess("Super Auto Pets.exe")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin org.phantombadger.plugins.superautopetsmod is loaded!");

            // Set up components
            AddComponent<TestMonoBehaviour>();
            AddComponent<EmoteConsumerMonoBehaviour>();
            AddComponent<EmoteDisplayMonoBehaviour>();
            AddComponent<ThreadDispatcher>();

            // Initialise Logger and Settings
            var logger = new BepInExLogger(Log);
            var userSettings = new UserSettings(SuperAutoPetsModSettingsContext.SettingsFileName, SuperAutoPetsModSettingsContext.GetDefaultSettings(), logger);

            // Initialise Harmony
            var harmony = new Harmony("com.phantombadger.superautopets");
            harmony.PatchAll();

            // Make the Twitch Factory
            var twitchClientFactory = new TwitchClientFactory(userSettings, logger);
            var twitchEmoteProvider = new TwitchEmoteProvider(twitchClientFactory.GetTwitchClient(), logger);

            // Set up the manual patches
            var patch = new EmoteConsumerCreatorPatch(twitchEmoteProvider, logger);
            patch.SetUpManualPatch(harmony);

            Log.LogInfo($"Plugin org.phantombadger.plugins.superautopetsmod Load Function is complete!");
        }
    }
}
