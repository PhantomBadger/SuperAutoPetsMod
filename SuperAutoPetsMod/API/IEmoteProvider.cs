using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Models;

namespace SuperAutoPetsMod.API
{
    public interface IEmoteProvider
    {
        bool TryGetEmote(out Emote emote);
    }
}
