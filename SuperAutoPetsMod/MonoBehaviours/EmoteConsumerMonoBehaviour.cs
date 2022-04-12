using SuperAutoPetsMod.API;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TwitchLib.Client.Models;
using UnhollowerBaseLib;
using UnityEngine;
using ILogger = Logging.API.ILogger;

namespace SuperAutoPetsMod.MonoBehaviours
{
    /// <summary>
    /// A custom MonoBehaviour which polls for queued up emotes
    /// </summary>
    public class EmoteConsumerMonoBehaviour : MonoBehaviour
    {
        public IEmoteProvider EmoteProvider;
        public ILogger Logger;

        /// <summary>
        /// Called every frame by Unity
        /// </summary>
        public void Update()
        {
            if (EmoteProvider == null || Logger == null)
            {
                return;
            }

            if (EmoteProvider.TryGetEmote(out Emote emote))
            {
                // Do something with it
                Logger.Information($"Got Emote {emote.ImageUrl}");

                Texture2D tex = GetEmoteImage(emote.ImageUrl);
                Sprite sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);

                // Create Object to house the emote
                GameObject childObject = new GameObject();
                float xOffset = UnityEngine.Random.Range(-0.5f, 0.25f);
                childObject.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
                childObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                EmoteDisplayMonoBehaviour display = childObject.AddComponent<EmoteDisplayMonoBehaviour>();
                display.Logger = Logger;
                display.Sprite = sprite;
                Logger.Information($"Made & Assigned Sprite!");
            }
        }

        /// <summary>
        /// Gets the Texture at the specified url asynchronously
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Texture2D GetEmoteImage(string url)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] imageSrc = wc.DownloadData(new Uri(url));

                    Texture2D tex2D = new Texture2D(28, 28);
                    bool result = ImageLoader.LoadImage(tex2D, imageSrc, false);
                    return tex2D;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                return null;
            }
        }
    }
}
