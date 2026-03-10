using System;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    [Serializable] public class RenderTexturePoolItem : IDisposable
    {
        public RenderTexture RenderTexture;
        public bool Used;

        public RenderTexturePoolItem(RenderTexture renderTexture, bool used = false)
        {
            RenderTexture = renderTexture;
            Used = used;
        }

        public void Dispose()
        {
            if (RenderTexture != null)
            {
                RenderTexture.Release();
                UnityEngine.Object.Destroy(RenderTexture);
            }
        }
    }
}