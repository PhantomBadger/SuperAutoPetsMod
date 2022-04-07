using Logging.API;
using SuperAutoPetsMod.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SuperAutoPetsMod.Twitch
{
    public class TwitchEmoteProvider : IEmoteProvider, IDisposable
    {
        private readonly ConcurrentQueue<Emote> emoteQueue;
        private readonly TwitchClient twitchClient;
        private readonly ILogger logger;

        public TwitchEmoteProvider(TwitchClient twitchClient, ILogger logger)
        {
            this.twitchClient = twitchClient ?? throw new ArgumentNullException(nameof(twitchClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            emoteQueue = new ConcurrentQueue<Emote>();

            twitchClient.OnMessageReceived += OnMessageReceived;
        }

        public void Dispose()
        {
            twitchClient.OnMessageReceived -= OnMessageReceived;
        }

        public bool TryGetEmote(out Emote emote)
        {
            return emoteQueue.TryDequeue(out emote);
        }

        private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            logger.Warning(e.ChatMessage.Message);
        }
    }
}
