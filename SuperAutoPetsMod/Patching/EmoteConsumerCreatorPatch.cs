using HarmonyLib;
using Logging.API;
using Spacewood.Core.Models;
using Spacewood.Unity.Views;
using SuperAutoPetsMod.API;
using SuperAutoPetsMod.MonoBehaviours;
using SuperAutoPetsMod.Twitch;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SuperAutoPetsMod.Patching
{
    /// <summary>
    /// An implementation of <see cref="IManualPatch"/> which adds an <see cref="EmoteConsumerMonoBehaviour"/> to all created Pets
    /// </summary>
    public class EmoteConsumerCreatorPatch : IManualPatch
    {
        private static TwitchEmoteProvider EmoteProvider;
        private static ILogger Logger;

        public EmoteConsumerCreatorPatch(TwitchEmoteProvider emoteProvider, ILogger logger)
        {
            EmoteProvider = emoteProvider;
            Logger = logger;
        }

        public void SetUpManualPatch(Harmony harmony)
        {
            MethodInfo setMinionMethod = typeof(Spacewood.Unity.Views.BoardView).GetMethod("SetMinion");
            Logger.Information($"PATCH - Souce method: {setMinionMethod}");
            MethodInfo postfixMethod = AccessTools.Method("SuperAutoPetsMod.Patching.EmoteConsumerCreatorPatch:PostfixPatchMethodBoardSetMinion");
            Logger.Information($"PATCH - Target method: {postfixMethod}");
            harmony.Patch(setMinionMethod, postfix: new HarmonyMethod(postfixMethod));
            Logger.Information($"PATCH - DONE");
        }

        public static void PostfixPatchMethodBoardSetMinion(object __instance, ref MinionView __result, MinionModel minionModel)
        {
            try
            {
                if ((__result.gameObject.GetComponent<EmoteConsumerMonoBehaviour>() == null))
                {
                    EmoteConsumerMonoBehaviour consumer = __result.gameObject.AddComponent<EmoteConsumerMonoBehaviour>();

                    consumer.EmoteProvider = EmoteProvider;
                    consumer.Logger = Logger;

                    Logger.Information("Added EmoteConsumerMonoBehaviour to minion");
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Encountered Exception when attempting to add EmoteConsumerMonoBehaviour component: {e.ToString()}");
            }
        }
    }
}
