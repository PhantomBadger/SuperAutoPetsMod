using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ILogger = Logging.API.ILogger;

namespace SuperAutoPetsMod.MonoBehaviours
{
    /// <summary>
    /// A <see cref="MonoBehaviour"/> which moves the object containing the emote up and destroys it when done
    /// </summary>
    public class EmoteDisplayMonoBehaviour : MonoBehaviour
    {
        public ILogger Logger;
        public Sprite Sprite;

        private float startY;
        private float speed;

        private float movementDelay;
        private float delayCounter;
        private bool isDelayed;
        private SpriteRenderer spriteRenderer;

        private const float SpeedMin = 1.0f;
        private const float SpeedMax = 2.0f;
        private const float Distance = 0.01f;
        private const float FadeStep = 0.005f;

        public void Start()
        {
            startY = transform.position.y;
            speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);

            movementDelay = UnityEngine.Random.Range(0, 0.5f);
            delayCounter = 0;
            isDelayed = true;
        }

        public void Update()
        {
            if (Logger == null || Sprite == null)
            {
                return;
            }

            // Delay the rendering of the emote to add an implicit offset
            if (isDelayed)
            {
                if ((delayCounter += Time.deltaTime) > movementDelay)
                {
                    isDelayed = false;

                    spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = Sprite;
                }
                else
                {
                    return;
                }
            }

            // Travel up by the given speed
            Vector3 curPos = transform.position;
            curPos.y += Distance * speed;
            transform.position = curPos;

            // Fade by a given amount
            Color curCol = spriteRenderer.color;
            curCol.a -= FadeStep;
            spriteRenderer.color = curCol;

            // if far enough away, Destroy
            if (Mathf.Abs(curPos.y - startY) > 5)
            {
                Destroy(this);
            }
        }
    }
}
