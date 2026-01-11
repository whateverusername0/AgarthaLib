using AgarthaLib.Extensions;
using UnityEngine;

namespace AgarthaLib.Sprites.Layers.Rendering
{
    public class LayeredSpriteRenderer : LayeredSpriteRendererBase
    {
        protected override void CreateLayer(SpriteLayer layer)
        {
            var go = new GameObject(layer.Name);
            go.transform.SetParent(transform);
            go.AddComponent<SpriteRenderer>();
            UpdateLayer(layer);
        }

        protected override void UpdateLayer(SpriteLayer layer)
        {
            var child = transform.GetChildByName(layer.Name);
            if (child == null) return;

            if (!child.TryGetComponent<SpriteRenderer>(out var sr))
                return;

            sr.sprite = layer.Sprite;
            sr.material = layer.Material != null ? layer.Material : sr.material;
        }

        protected override void SortLayers()
        {
            var layers = transform.GetChildren();
            var map = Sprite.LayerMap;

            // sorting
            foreach (var l in layers)
            {
                if (!map.TryGetLayer(l.name, out var layerData)
                || !l.TryGetComponent<SpriteRenderer>(out var sr))
                    continue;

                sr.sortingOrder = map.Map.IndexOf(layerData);
            }
        }
    }
}