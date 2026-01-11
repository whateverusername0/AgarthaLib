using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Sprites.Layers
{
    [Serializable] public class SpriteLayerMap
    {
        public readonly List<SpriteLayer> Map = new();

        public bool TryGetLayer(string name, out SpriteLayer layer)
        {
            layer = Map.Where(q => q.Name == name).FirstOrDefault();
            return layer != null && layer != default;
        }

        public bool HasLayer(string layer)
            => Map.Any(q => q.Name == layer);

        public void SetSprite(string layer, Sprite sprite)
        {
            if (!TryGetLayer(layer, out var l))
                l = AddLayer(layer, sprite);

            if (l != null && l != default)
                l.Sprite = sprite;
        }

        public SpriteLayer AddLayer(SpriteLayer layer)
        {
            if (Map.Contains(layer)) return layer;

            Map.Add(layer);
            return layer;
        }

        public SpriteLayer AddLayer(string name, Sprite sprite)
        {
            if (TryGetLayer(name, out var existing))
                return existing;

            var l = new SpriteLayer(name, sprite);
            Map.Add(l);
            return l;
        }
    }

    [Serializable] public class SpriteLayer
    {
        public readonly string Name;
        public Sprite Sprite;
        public Material Material;

        public SpriteLayer(string name, Sprite sprite, Material material = null)
        {
            Name = name;
            Sprite = sprite;
            Material = material;
        }

        public static implicit operator string(SpriteLayer sl)
            => sl.Name;

        public static implicit operator Sprite(SpriteLayer sl)
            => sl.Sprite;
    }
}
