using Logging.API;
using Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Unity;

namespace SuperAutoPetsMod.Twitch
{
    /// <summary>
    /// A class whose purpose is to make a <see cref="TwitchClient"/>
    /// </summary>
    public class TwitchClientFactory
    {
        private readonly ILogger logger;
        private readonly UserSettings userSettings;

        private TwitchClient twitchClient;

        /// <summary>
        /// Constructor for creating a <see cref="TwitchClientFactory"/>
        /// </summary>
        /// <param name="userSettings">A <see cref="UserSettings"/> class to get settings info from</param>
        /// <param name="logger">An <see cref="ILogger"/> implementation for logging</param>
        public TwitchClientFactory(UserSettings userSettings, ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));

            twitchClient = null;
        }

        ~TwitchClientFactory()
        {
            logger.Information("a");
        }

        /// <summary>
        /// Gets a <see cref="TwitchClient"/> instance for interfacing with twitch chat
        /// </summary>
        public TwitchClient GetTwitchClient()
        {
            if (twitchClient != null)
            {
                return twitchClient;
            }
            else
            {
                // Initialise the settings and attempt to load the OAuth Token
                string oAuthToken = userSettings.GetSettingOrDefault(SuperAutoPetsModSettingsContext.OAuthKey, string.Empty);
                string twitchName = userSettings.GetSettingOrDefault(SuperAutoPetsModSettingsContext.ChatListenerTwitchAccountNameKey, string.Empty);

                // If the Oauth Token is bad, exit now
                if (string.IsNullOrWhiteSpace(oAuthToken))
                {
                    logger.Error($"No valid OAuth token found in the {SuperAutoPetsModSettingsContext.SettingsFileName} file!");
                    return null;
                }
                if (string.IsNullOrWhiteSpace(twitchName))
                {
                    logger.Error($"No valid TwitchAccountName found in the {SuperAutoPetsModSettingsContext.SettingsFileName} file!");
                    return null;
                }

                oAuthToken = oAuthToken.Trim();
                twitchName = twitchName.Trim();
                logger.Information($"Setting up Twitch Chat Client for '{twitchName}'");

                try
                {
                    var credentials = new ConnectionCredentials(twitchName, oAuthToken);
                    var twitchClient = new Client();
                    twitchClient.Initialize(credentials, twitchName);

                    twitchClient.Connect();

                    return twitchClient;
                }
                catch (Exception e)
                {
                    logger.Information(e.ToString());
                    return null;
                }
            }
        }
    }
}
