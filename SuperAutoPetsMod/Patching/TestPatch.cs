using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using Spacewood.Core.Enums;
using Spacewood.Core.Models;
using Spacewood.Core.System;
using Spacewood.Unity.Views;
using SuperAutoPetsMod.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SuperAutoPetsMod.Patching
{
    public class TestPatch : IManualPatch
    {
        private static ManualLogSource logSource;
        private static ConcurrentDictionary<string, Color> lookup;
        private static ConcurrentDictionary<string, GameObject> lookup2;

        public TestPatch(ManualLogSource newLogger)
        {
            logSource = newLogger ?? throw new ArgumentNullException(nameof(newLogger));
            lookup = new ConcurrentDictionary<string, Color>();
            lookup2 = new ConcurrentDictionary<string, GameObject>();
        }

        public void SetUpManualPatch(Harmony harmony)
        {
            //var method = typeof(Spacewood.Core.Actions.Board.BoardExtensions).GetMethod("GenerateMinion", new[] { typeof(BoardModel), typeof(MinionEnum), typeof(int), typeof(Owner), typeof(Location), typeof(IRandom) });
            //logSource.LogInfo($"PATCH - Souce method: {method}");
            //var postfixMethod = AccessTools.Method("SuperAutoPetsMod.Patching.TestPatch:PostfixPatchMethodGenerate");
            //logSource.LogInfo($"PATCH - Target method: {postfixMethod}");
            //harmony.Patch(method, postfix: new HarmonyMethod(postfixMethod));
            //logSource.LogInfo($"PATCH - DONE");

            var method2 = typeof(Spacewood.Unity.Views.BoardView).GetMethod("SetMinion");
            logSource.LogInfo($"PATCH2 - Souce method: {method2}");
            var postfixMethod2 = AccessTools.Method("SuperAutoPetsMod.Patching.TestPatch:PostfixPatchMethodBoardSetMinion");
            logSource.LogInfo($"PATCH2 - Target method: {postfixMethod2}");
            harmony.Patch(method2, postfix: new HarmonyMethod(postfixMethod2));
            logSource.LogInfo($"PATCH2 - DONE");

            //var ctor = typeof(Spacewood.Unity.Views.MinionView).GetMethod("Awake");
            //logSource.LogInfo($"PATCH3 - Source method: {ctor}");
            //var postfixMethod3 = AccessTools.Method("SuperAutoPetsMod.Patching.TestPatch:PostfixPatchMethodCtor");
            //logSource.LogInfo($"PATCH3 - Target method: {postfixMethod3}");
            //harmony.Patch(ctor, postfix: new HarmonyMethod(postfixMethod3));
            //logSource.LogInfo($"PATCH3 - DONE");
        }

        public static void PostfixPatchMethodGenerate(object __instance, MinionModel __result, BoardModel board, MinionEnum minionEnum, int level, Owner owner, Location location, IRandom random)
        {
            // This generates the Minion backend (Model)
            // it doesnt bind to any visual nor any positional data so isnt very useful for what we want
            try
            {
                logSource.LogInfo($"PostfixPatchMethodGenerate was Called! Flagged from Patch! MinionEnum: '{minionEnum.ToString()}' Owner: '{owner.ToString()}' Location '{location.ToString()}'");

                StringBuilder minion = new StringBuilder();
                minion.Append($"Il2CppType: {MinionModel.Il2CppType.Name}\n");
                minion.Append($"Discounted: {__result.Discounted}\n");
                minion.Append($"Tier: {__result.Tier}\n");
                minion.Append($"DragAndDropMode: {__result.DragAndDropMode}\n");
                //minion.Append($"DestroyedBy: {(__result.DestroyedBy != null && __result.DestroyedBy.HasValue ? __result.DestroyedBy.Value.ToString() : "")}\n");
                minion.Append($"Dead: {__result.Dead}\n");
                minion.Append($"Cosmetic: {__result.Cosmetic.ToString()}\n");
                minion.Append($"Level: {__result.Level}\n");
                minion.Append($"Exp: {__result.Exp}\n");
                minion.Append($"Health: {__result.Health.Permanent}\n");
                minion.Append($"Attack: {__result.Attack.Permanent}\n");
                minion.Append($"Template:\n");
                minion.Append($"\t Il2CppType: {MinionTemplate.Il2CppType.Name}\n");
                minion.Append($"\t Rewardless: {__result.Template.Rewardless}\n");
                minion.Append($"\t Elite: {__result.Template.Elite}\n");
                minion.Append($"\t Health: {__result.Template.Health}\n");
                minion.Append($"\t Attack: {__result.Template.Attack}\n");
                minion.Append($"\t Name: {__result.Template.Name}\n");
                minion.Append($"\t Enum: {__result.Template.Enum.ToString()}\n");
                //minion.Append($"\t Perk: {(__result.Template.Perk != null && __result.Template.Perk.HasValue ? __result.Template.Perk.Value.ToString() : "")}\n");
                //minion.Append($"\t PerkAbout: {(__result.Template.PerkAbout != null && __result.Template.PerkAbout.HasValue ? __result.Template.PerkAbout.Value.ToString() : "")}\n");
                //minion.Append($"\t Archetype: {(__result.Template.Archetype != null && __result.Template.Archetype.HasValue ? __result.Template.Archetype.Value.ToString() : "")}\n");
                minion.Append($"Point:\n");
                minion.Append($"\t Il2CppType: {Point.Il2CppType.Name}\n");
                minion.Append($"\t X: {__result.Point.x}\n");
                minion.Append($"\t Y: {__result.Point.y}\n");
                logSource.LogInfo(minion.ToString());

                logSource.LogInfo($"Minions:");
                for (int i = 0; i < board.Minions.Items.Count; i++)
                {
                    logSource.LogInfo($"i {i} min {board?.Minions?.Items[i]?.Enum.ToString()} x {board?.Minions?.Items[i]?.Point.x} y {board?.Minions?.Items[i]?.Point.x}");
                }

                logSource.LogInfo($"MinionsShop:");
                for (int i = 0; i < board.MinionShop.Count; i++)
                {
                    logSource.LogInfo($"i {i} min {board?.MinionShop[i]?.Enum.ToString()} x {board?.MinionShop[i]?.Point.x} y {board?.MinionShop[i]?.Point.x}");
                }

                //System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                //logSource.LogInfo($"StackTrace is: {t.ToString()}");
                logSource.LogInfo($"End of Patch method");
            }
            catch (Exception e)
            {
                logSource.LogInfo($"Failed to run patch: {e.ToString()}");
            }
        }

        public static void PostfixPatchMethodBoardSetMinion(object __instance, ref MinionView __result, MinionModel minionModel)
        {
            // Called when a Minion is added to the board!
            try
            {
                logSource.LogInfo($"Entered Board.SetMinion");
                logSource.LogInfo($"\tPosX {__result.transform.position.x}, PosY {__result.transform.position.y}, Parent? '{__result.transform.parent}' ViewMode {__result.Mode.ToString()}");
                logSource.LogInfo($"\tMinionModelEnum {__result.Minion.Enum.ToString()}");
                logSource.LogInfo($"\tSprite {__result.SpriteRenderer.sprite?.name}");
                logSource.LogInfo($"\tId {__result.MinionId.Unique} {__result.MinionId.ReadableID} {__result.MinionId.BoardId}");
                logSource.LogInfo($"Exit Board.SetMinion");

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
                        logSource.LogInfo("Added TestMonoBehaviour to minion");
                    }
                }
            }
            catch (Exception e)
            {
                logSource.LogInfo($"Exception during Board.SetMinion {e.ToString()}");
            }
        }

        //public static void PostfixPatchMethodCtor(object __instance)
        //{
        //    logSource.LogInfo($"Ctor Called");
        //    MinionView view = __instance as MinionView;
        //    System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
        //    logSource.LogInfo(t.ToString());
        //    logSource.LogInfo($"Casted? {view?.GetType().Name}");

        //        logSource.LogInfo($"\tPosX {view.transform.position.x}, PosY {view.transform.position.y}, Parent? '{view.transform.parent}' ViewMode {view.Mode.ToString()}");
        //        logSource.LogInfo($"\tMinionModelEnum {view.Mini.MinionName}");
        //        logSource.LogInfo($"\tSprite {view.SpriteRenderer.sprite?.name}");

        //    logSource.LogInfo($"Ctor End");
        //}
    }
}
