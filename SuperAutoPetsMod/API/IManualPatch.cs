using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperAutoPetsMod.API
{
    /// <summary>
    /// Interface representing a manual patch of some game code using Harmony
    /// </summary>
    public interface IManualPatch
    {
        /// <summary>
        /// Sets up the manual patching using the harmony instance provided
        /// </summary>
        void SetUpManualPatch(Harmony harmony);
    }
}
