using BepInEx.Logging;
using HarmonyLib;
using Logging.API;
using Spacewood.Core.Models;
using Spacewood.Unity.Views;
using SuperAutoPetsMod.API;
using SuperAutoPetsMod.Twitch;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SuperAutoPetsMod.Patching
{
    public class BoardViewPatch : IManualPatch
    {
        private static ILogger logger;
        private static TwitchEmoteProvider emoteProvider;

        public BoardViewPatch(TwitchEmoteProvider emoteProvider, ILogger logger)
        {
            BoardViewPatch.emoteProvider = emoteProvider;
            BoardViewPatch.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetUpManualPatch(Harmony harmony)
        {
            MethodInfo sourceMethod = typeof(Spacewood.Unity.Views.BoardView).GetMethod("SetMinion");
            MethodInfo postfixMethod = typeof(SuperAutoPetsMod.Patching.BoardViewPatch).GetMethod("PostfixPatchBoardViewSetMinion");
        }

        public static void PostfixPatchBoardViewSetMinion(object __instance, ref MinionView __result, MinionModel minionModel)
        {
            // TODO: Make consumer component and give it the provider
        }
    }
}
