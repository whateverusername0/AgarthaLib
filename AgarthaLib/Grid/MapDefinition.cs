using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Grid
{
    public class MapDefinition : AgarthanBehaviour
    {
        public List<MapLayerDefinition> Layers = new();

        public MapLayerDefinition GetLayerInMap(string name)
            => Layers.Where(q => q != null && q.Name == name).FirstOrDefault();

        public MapLayerDefinition GetLayer(string name)
        {
            var layer = GetLayerInMap(name);
            if (layer != null) return layer;

            var child = transform.GetChildren<TilemapDefinition>().Where(q => q.name == name).FirstOrDefault();
            if (child != null) return AddLayer(child.name, child);

            return null;
        }

        public MapLayerDefinition AddLayer(MapLayerDefinition layer)
        {
            var existing = GetLayerInMap(name);
            if (existing != null) return existing;

            Layers.Add(layer);
            return layer;
        }

        public MapLayerDefinition AddLayer(string name, TilemapDefinition tilemap)
            => AddLayer(new MapLayerDefinition(name, tilemap));

        public MapLayerDefinition AddLayer(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);
            var td = go.AddComponent<TilemapDefinition>();
            td.MapDefinition = this;
            return AddLayer(name, td);
        }

        public void DeleteLayer(MapLayerDefinition layer)
        {
            Layers.Remove(layer);
            Destroy(layer.Tilemap.gameObject); // good times!
        }

        public void DeleteLayer(string name)
        {
            var layer = GetLayerInMap(name);
            if (layer == null) return;

            DeleteLayer(layer);
        }

        public TileData GetTile(Vector3 position)
        {
            foreach (var layer in Layers)
            {
                var tile = layer.Tilemap.GetTile(position);
                if (tile != null) return tile;
            }
            return null;
        }

        public bool IsWalkable(Vector3 position)
            => !GetTile(position).Tilemap.ProvideCollisions;
    }

    [Serializable] public class MapLayerDefinition
    {
        public string Name;
        public TilemapDefinition Tilemap;

        public MapLayerDefinition(string name, TilemapDefinition tilemap)
        {
            Name = name;
            Tilemap = tilemap;
        }
    }
}