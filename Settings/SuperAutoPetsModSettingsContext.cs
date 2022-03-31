using System;
using System.Collections.Generic;
using System.Text;

namespace Settings
{
    public abstract class SuperAutoPetsModSettingsContext
    {
        public const string SettingsFileName = "SuperAutoPetsMod.settings";
        public const char CommentCharacter = '#';

        // Twitch
        public const string ChatListenerTwitchAccountNameKey = "ChatListenerTwitchAccountName";
        public const string OAuthKey = "OAuth";

        public static Dictionary<string, string> GetDefaultSettings()
        {
            return new Dictionary<string, string>()
            {
                // Twitch Chat
                { ChatListenerTwitchAccountNameKey, "" },
                { OAuthKey, "" },
            };
        }
    }
}
