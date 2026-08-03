using AgarthaLib._2D.Tilemaps;
using AgarthaLib.Attributes;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace AgarthaLib._2D.Grids
{
    public abstract class MapGrid<TTilemap, TLayer> : AgarthanBehaviour
        where TTilemap : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        [SerializeField, EditorReadOnly]
        protected SerializedDictionary<TLayer, TTilemap> _layers = new();

        public virtual SerializedDictionary<TLayer, TTilemap> Layers
            => _layers;

        public virtual Map<MapGrid<TTilemap, TLayer>, TTilemap, TLayer> GetMap()
            => GetComponentInParent<Map<MapGrid<TTilemap, TLayer>, TTilemap, TLayer>>();

        public virtual TTilemap GetTilemap(TLayer layer)
            => Layers.TryGetValue(layer, out var map) ? map : null;

        public abstract LayerData GetLayerData(TLayer layer);

        protected override void Start()
        {
            base.Start();

            ResolveLayers();
        }

        public virtual void ResolveLayers()
        {
            foreach (TLayer layer in Enum.GetValues(typeof(TLayer)))
            {
                if (!_layers.ContainsKey(layer))
                    _layers.Add(layer, null);

                if (_layers[layer] == null)
                    _layers[layer] = CreateLayer(layer);

                var layerData = GetLayerData(layer);
                if (layerData == null) continue;
                ConfigureLayer(layer, _layers[layer], layerData);
            }
        }

        protected virtual TTilemap CreateLayer(TLayer layer, bool configure = false)
        {
            var go = new GameObject($"layer_{layer}");
            go.transform.SetParent(this.transform, false);

            var td = go.AddComponent<TTilemap>();
            td.SetLayer(layer);
            td.Tilemap = td.EnsureComponent<Tilemap>();

            if (configure)
            {
                var layerData = GetLayerData(layer);
                if (layerData == null) return td;
                ConfigureLayer(layer, td, layerData);
            }
            
            return td;
        }

        protected virtual void ConfigureLayer(TLayer layer, TTilemap td, LayerData layerData)
        {
            if (layerData.ShouldRender)
            {
                var renderer = td.EnsureComponent<TilemapRenderer>();
                renderer.sortingOrder = (int)(object)layer;
                renderer.sortingLayerID = layerData.SortingLayer.id;

                if (layerData.RenderMaterial != null)
                    renderer.material = layerData.RenderMaterial;
            }

            if (layerData.ProvidesCollision)
            {
                var collision = td.EnsureComponent<TilemapCollider2D>();
                collision.isTrigger = layerData.IsTrigger;
                td.EnsureComponent<TilemapCollider3D>();

                var map = GetMap();
                if (layerData.CollisionLayer != 0 && map != null)
                {
                    var collisionLayer = layerData.CollisionLayer;
                    collision.includeLayers = collisionLayer;
                    collision.excludeLayers = collisionLayer.Inverted();
                }
            }
        }

        #region ManagedTilemap Extensions

        public virtual TileBase GetTile(TLayer layer, Vector2Int pos)
            => GetTilemap(layer).GetTile(pos);

        public virtual TTile GetTile<TTile>(TLayer layer, Vector2Int pos)
            where TTile : TileBase
            => GetTilemap(layer).GetTile<TTile>(pos);

        public virtual bool TryGetTile(TLayer layer, Vector2Int pos, out TileBase tile)
            => GetTilemap(layer).TryGetTile(pos, out tile);

        public virtual bool TryGetTile<TTile>(TLayer layer, Vector2Int pos, out TTile ttile)
            where TTile : TileBase
            => GetTilemap(layer).TryGetTile(pos, out ttile);

        public virtual bool TileExists(TLayer layer, Vector2Int pos)
            => GetTilemap(layer).TileExists(pos);

        public virtual Vector2Int WorldToTile(TLayer layer, Vector2 pos)
            => GetTilemap(layer).WorldToTile(pos);

        public virtual Vector2 TileToWorld(TLayer layer, Vector2Int pos)
            => GetTilemap(layer).TileToWorld(pos);

        public virtual void SetTile(TLayer layer, Vector2Int pos, TileBase tile)
            => GetTilemap(layer).SetTile(pos, tile);

        public List<(Vector2Int position, TileBase tile)> GetAllTiles(TLayer layer, bool notNull = false)
            => GetTilemap(layer).GetAllTiles(notNull);

        public List<(Vector2Int position, T tile)> GetAllTilesOfType<T>(TLayer layer) where T : TileBase
            => GetTilemap(layer).GetAllTilesOfType<T>();

        public List<(Vector2Int position, TileBase tile)> GetTilesInRange(TLayer layer, Vector2Int position,
            int range)
            => GetTilemap(layer).GetTilesInRange(position, range);

        public List<(Vector2Int position, TileBase tile)> GetAdjacentTiles(TLayer layer, Vector2Int position,
            bool allowDiagonal)
            => GetTilemap(layer).GetAdjacentTiles(position, allowDiagonal);

        public List<(Vector2Int position, TileBase tile)> GetConnectedTiles(TLayer layer, Vector2Int position,
            TileBase tile, bool allowDiagonal)
            => GetTilemap(layer).GetConnectedTiles(position, tile, allowDiagonal);

        #endregion

        public (TLayer layer, Vector2Int position, TileBase tile)? GetHighestTile(Vector2Int position)
        {
            var layers = GetAllTilesOn(position);
            if (layers == null || layers.Count == 0) return null;
            return layers.Last(); // it's ordered by ascending
        }

        public List<(TLayer layer, Vector2Int position, TileBase tile)> GetAllTilesOn(Vector2Int position)
        {
            var layers = NETExtensions.GetEnumValues<TLayer>();
            var l = new List<(TLayer, Vector2Int, TileBase)>();

            foreach (var layer in layers)
                if (Layers.TryGetValue(layer, out var lt))
                    l.Add((layer, position, lt.GetTile(position)));

            return l;
        }
    }
}
