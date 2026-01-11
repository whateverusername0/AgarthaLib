using AgarthaLib.Sprites.Layers.Rendering;
using AgarthaLib.Sprites.Layers;
using UnityEngine;
using UnityEngine.UI;
using AgarthaLib.Extensions;

public class LayeredRawImageRenderer : LayeredSpriteRendererBase
{
    protected override void CreateLayer(SpriteLayer layer)
    {
        var go = new GameObject(layer.Name);
        go.transform.SetParent(transform);
        go.AddComponent<RawImage>();
        UpdateLayer(layer);
    }

    protected override void UpdateLayer(SpriteLayer layer)
    {
        var child = transform.GetChildByName(layer.Name);
        if (child == null) return;

        if (!child.TryGetComponent<RawImage>(out var sr))
            return;

        sr.texture = layer.Sprite.texture;
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
            || !l.TryGetComponent<RawImage>(out var sr))
                continue;

            var index = map.Map.IndexOf(layerData).Reverse(map.Map.Count);
            sr.transform.SetSiblingIndex(index);
        }
    }
}