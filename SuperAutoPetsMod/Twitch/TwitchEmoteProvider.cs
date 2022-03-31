using SuperAutoPetsMod.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SuperAutoPetsMod.Twitch
{
    public class TwitchEmoteProvider : IEmoteProvider
    {
        private readonly ConcurrentQueue<Emote> emoteQueue;
        private readonly TwitchClient twitchClient;

        public TwitchEmoteProvider(TwitchClient twitchClient)
        {
            this.twitchClient = twitchClient ?? throw new ArgumentNullException(nameof(twitchClient));
            emoteQueue = new ConcurrentQueue<Emote>();
        }

        public bool TryGetEmote(out Emote emote)
        {
            return emoteQueue.TryDequeue(out emote);
        }
    }
}
