using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Goodies.Portals
{
    public class RenderTexturePool : AgarthanSingleton<RenderTexturePool>
    {
        [SerializeField, EditorReadOnly] private List<RenderTexturePoolItem> _pool = new();

        public int MaxSize = 64;

        public RenderTexturePoolItem GetTexture()
        {
            var first = _pool.FirstOrDefault(q => !q.Used);
            if (first != null) return first;

            if (_pool.Count >= MaxSize)
            {
                var err = $"{typeof(RenderTexturePool)}.{nameof(_pool)} is full!";
                Debug.LogError(err);
                throw new OverflowException(/*err*/);
            }

            var @new = CreateTexture();
            _pool.Add(@new);
            @new.RenderTexture.name = _pool.Count.ToString();
            @new.Used = true;

            Debug.Log($"New {typeof(RenderTexture)} created. Pool is now this ({_pool.Count}) big.");

            return @new;
        }

        private RenderTexturePoolItem CreateTexture()
        {
            var rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
            rt.Create();
            return new RenderTexturePoolItem(rt);
        }

        public void ReleaseTexture(RenderTexturePoolItem item)
        {
            if (item == null) return;
            item.Used = false;
        }

        public void ReleaseAllTextures()
        {
            foreach (var item in _pool)
                ReleaseTexture(item);
        }

        private void DestroyTexture(RenderTexturePoolItem item)
            => item.Dispose();

        private void OnDestroy()
        {
            foreach (var item in _pool)
                DestroyTexture(item);
        }
    }
}