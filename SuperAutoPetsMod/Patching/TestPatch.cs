using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using Spacewood.Core.Enums;
using Spacewood.Core.Models;
using Spacewood.Core.System;
using Spacewood.Unity.MonoBehaviours.Battle;
using Spacewood.Unity.MonoBehaviours.Board;
using Spacewood.Unity.MonoBehaviours.Build;
using Spacewood.Unity.Views;
using SuperAutoPetsMod.API;
using SuperAutoPetsMod.MonoBehaviours;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SuperAutoPetsMod.Patching
{
    public class TestPatch : IManualPatch
    {
        private static BepInExLogger logger;
        //private static ConcurrentDictionary<string, Color> lookup;
        //private static ConcurrentDictionary<string, GameObject> lookup2;

        public TestPatch(BepInExLogger newLogger)
        {
            logger = newLogger ?? throw new ArgumentNullException(nameof(newLogger));
            //lookup = new ConcurrentDictionary<string, Color>();
            //lookup2 = new ConcurrentDictionary<string, GameObject>();
        }

        public void SetUpManualPatch(Harmony harmony)
        {
            var method1 = typeof(Spacewood.Unity.Views.BoardView).GetMethod("SetMinion");
            logger.Information($"PATCH - Souce method: {method1}");
            var postfixMethod1 = AccessTools.Method("SuperAutoPetsMod.Patching.TestPatch:PostfixPatchMethodBoardSetMinion");
            logger.Information($"PATCH - Target method: {postfixMethod1}");
            harmony.Patch(method1, postfix: new HarmonyMethod(postfixMethod1));
            logger.Information($"PATCH - DONE");
        }

        public static void PostfixPatchMethodBoardSetMinion(object __instance, ref MinionView __result, MinionModel minionModel)
        {
            // Called when a Minion is added to the board!
            try
            {
                logger.Information($"Entered Board.SetMinion");
                logger.Information($"\tPosX {__result.transform.position.x}, PosY {__result.transform.position.y}, Parent? '{__result.transform.parent}' ViewMode {__result.Mode.ToString()}");
                logger.Information($"\tMinionModelEnum {__result.Minion.Enum.ToString()}");
                logger.Information($"\tSprite {__result.SpriteRenderer.sprite?.name}");
                logger.Information($"\tId '{__result.MinionId.Unique}' '{__result.MinionId.ReadableID}' '{__result.MinionId.BoardId}' '{__result.Minion.Id.ReadableID}' '{__result.Minion.Id.Unique}' '{__result.Minion.Id.BoardId}'");
                logger.Information($"Exit Board.SetMinion");

                // Test 1
                // make b i g
                {
                    //if (__result.Minion.Enum == MinionEnum.Pig)
                    //{
                    //    __result.transform.localScale = new Vector3(2, -2, 1);
                    //}
                    //else if (__result.Minion.Enum == MinionEnum.Fish)
                    //{
                    //    __result.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    //}
                }

                // Test 2
                // Make a unique colour per minion and track it between fights
                // the unique ID changes in battle, as its a new "BoardView"
                {
                    //var spriteRenderer = __result.gameObject.AddComponent<SpriteRenderer>();
                    //logSource.LogInfo($"Sprite Info '{spriteRenderer?.sprite?.texture?.name}' '{spriteRenderer?.sprite?.name}'");
                    //Texture2D tex = new Texture2D(50, 50);
                    //Color color;
                    //if (!lookup.ContainsKey(__result.MinionId.ReadableID))
                    //{
                    //    lookup[__result.MinionId.ReadableID] = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
                    //}
                    //color = lookup[__result.MinionId.ReadableID];
                    //spriteRenderer.sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);
                    //spriteRenderer.color = color;
                    //logSource.LogInfo($"Sprite Info '{spriteRenderer?.sprite?.texture?.name}' '{spriteRenderer?.sprite?.name}'");
                }

                // Test 3
                // Make an entirely new GameObject with the unique colour
                {
                    //if (!lookup2.ContainsKey(__result.MinionId.ReadableID))
                    //{
                    //    lookup2[__result.MinionId.ReadableID] = new GameObject();
                    //}
                    //GameObject go = lookup2[__result.MinionId.ReadableID];
                    //go.transform.parent = __result.transform;
                    //go.transform.position = Vector3.zero;
                    //go.transform.rotation = Quaternion.identity;
                    //go.transform.localScale = Vector3.one;

                    //var spriteRenderer = go.AddComponent<SpriteRenderer>();
                    ////logSource.LogInfo($"Sprite Info '{spriteRenderer?.sprite?.texture?.name}' '{spriteRenderer?.sprite?.name}'");
                    //Texture2D tex = new Texture2D(50, 50);
                    //Color color;
                    //if (!lookup.ContainsKey(__result.MinionId.ReadableID))
                    //{
                    //    lookup[__result.MinionId.ReadableID] = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
                    //}
                    //color = lookup[__result.MinionId.ReadableID];
                    //spriteRenderer.sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);
                    //spriteRenderer.color = color;
                    //logSource.LogInfo($"Sprite Info '{spriteRenderer?.sprite?.texture?.name}' '{spriteRenderer?.sprite?.name}'");
                }

                // Test 4
                // Make an entirely new MonoBehaviour Component and add it to the pet
                {
                    if ((__result.gameObject.GetComponent<TestMonoBehaviour>() == null))
                    {
                        __result.gameObject.AddComponent<TestMonoBehaviour>();
                        logger.Information("Added TestMonoBehaviour to minion");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Information($"Exception during Board.SetMinion {e.ToString()}");
            }
        }
    }
}
