using BepInEx.Logging;
using Logging.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperAutoPetsMod
{
    public class BepInExLogger : ILogger
    {
        private readonly ManualLogSource logSource;

        public BepInExLogger(ManualLogSource logSource)
        {
            this.logSource = logSource;
        }

        public void Error(string message)
        {
            logSource.LogError(message);
        }

        public void Information(string message)
        {
            logSource.LogInfo(message);
        }

        public void Warning(string message)
        {
            logSource.LogWarning(message);
        }
    }
}
