using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using SuperAutoPetsMod.Patching;

namespace MyFirstPlugin
{
    [BepInPlugin("org.phantombadger.plugins.superautopetsmod", "Super Auto Pets Mod", "0.0.0.1")]
    [BepInProcess("Super Auto Pets.exe")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin org.phantombadger.plugins.superautopetsmod is loaded!");

            AddComponent<TestMonoBehaviour>();

            var harmony = new Harmony("com.phantombadger.superautopets");
            harmony.PatchAll();

            var patch = new TestPatch(Log);
            patch.SetUpManualPatch(harmony);

            Log.LogInfo($"Plugin org.phantombadger.plugins.superautopetsmod Load Function is complete!");
        }
    }
}
