using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SuperAutoPetsMod.Patching
{
    public class TestMonoBehaviour : MonoBehaviour
    {
        float rotateAmount = 10;
        Vector3 startScale;

        public void Awake()
        {
            rotateAmount = UnityEngine.Random.value * 30;
            startScale = transform.localScale;
        }

        public void Update()
        {
            transform.Rotate(new Vector3(0, 0, rotateAmount));
            transform.localScale = (startScale * (Mathf.Sin(Time.time * 5) / 2)) + new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
