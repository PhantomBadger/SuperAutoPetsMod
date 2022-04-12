using Logging.API;
using SuperAutoPetsMod.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;
using ILogger = Logging.API.ILogger;

namespace SuperAutoPetsMod.Twitch
{
    /// <summary>
    /// An implementation of <see cref="IEmoteProvider"/> which collects the Emotes from the Twitch messages
    /// </summary>
    public class TwitchEmoteProvider : IEmoteProvider, IDisposable
    {
        private readonly ConcurrentQueue<Emote> emoteQueue;
        private readonly TwitchClient twitchClient;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for creating a <see cref="TwitchEmoteProvider"/>
        /// </summary>
        /// <param name="twitchClient">The <see cref="TwitchClient"/> to get messages from</param>
        /// <param name="logger">An implementation of <see cref="ILogger"/> to use for logging</param>
        public TwitchEmoteProvider(TwitchClient twitchClient, ILogger logger)
        {
            this.twitchClient = twitchClient ?? throw new ArgumentNullException(nameof(twitchClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            emoteQueue = new ConcurrentQueue<Emote>();

            twitchClient.OnMessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// An implementation of <see cref="IDisposable.Dispose"/> to clean up our internal events
        /// </summary>
        public void Dispose()
        {
            twitchClient.OnMessageReceived -= OnMessageReceived;
        }

        /// <summary>
        /// Attempts to get an emote from the internal queue
        /// </summary>
        public bool TryGetEmote(out Emote emote)
        {
            return emoteQueue.TryDequeue(out emote);
        }

        /// <summary>
        /// Called when receiving a message, queues up all emotes within
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            List<Emote> emotes = e.ChatMessage.EmoteSet.Emotes;
            for (int i = 0; i < emotes.Count; i++)
            {
                emoteQueue.Enqueue(emotes[i]);
                logger.Information($"Queueing up Emote: {emotes[i].Name}");
            }

            logger.Warning(e.ChatMessage.Message);
        }
    }
}
