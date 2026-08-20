using AgarthaLib._2D.Tiles;
using AgarthaLib.Attributes;
using AgarthaLib.Collision;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Grids
{
    public abstract class MapGrid<TGridLayer, TLayer> : AgarthanBehaviour
        where TGridLayer : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        [SerializeField, EditorReadOnly]
        protected SerializedDictionary<TLayer, TGridLayer> _layers = new();

        public virtual SerializedDictionary<TLayer, TGridLayer> Layers
            => _layers;

        public virtual Map<MapGrid<TGridLayer, TLayer>, TGridLayer, TLayer> GetMap()
            => GetComponentInParent<Map<MapGrid<TGridLayer, TLayer>, TGridLayer, TLayer>>();

        public virtual TGridLayer GetTilemap(TLayer layer)
            => Layers.TryGetValue(layer, out var map) ? map : null;

        public abstract LayerData GetLayerData(TLayer layer);

        public bool IsStatic = false;

        protected override void Start()
        {
            base.Start();

            ResolveLayers();
        }

        public virtual List<TGridLayer> ResolveLayers()
        {
            var l = new List<TGridLayer>();
            foreach (TLayer layer in Enum.GetValues(typeof(TLayer)))
            {
                if (!_layers.ContainsKey(layer))
                    _layers.Add(layer, null);

                if (_layers[layer] == null)
                    _layers[layer] = CreateLayer(layer);

                var layerData = GetLayerData(layer);
                if (layerData == null) continue;
                ConfigureLayer(layer, _layers[layer], layerData);

                l.Add(_layers[layer]);
            }
            return l;
        }

        protected virtual TGridLayer CreateLayer(TLayer layer, bool configure = false)
        {
            var go = new GameObject("layer");
            go.transform.SetParent(this.transform, false);

            var td = go.AddComponent<TGridLayer>();
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

        protected virtual void ConfigureLayer(TLayer layer, TGridLayer td, LayerData layerData)
        {
            var layerInt = (int)(object)layer;

            td.gameObject.name = $"layer_{layer}";
            td.transform.SetSiblingIndex(layerInt);

            if (layerData.ShouldRender)
            {
                var renderer = td.EnsureComponent<TilemapRenderer>();
                renderer.sortingOrder = layerInt;
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

        public virtual Vector2Int WorldToTile(Vector2 pos)
            => _layers.First().Value.WorldToTile(pos);

        public virtual Vector2 TileToWorld(Vector2Int pos)
            => _layers.First().Value.TileToWorld(pos);

        public virtual void SetTile(TLayer layer, Vector2Int pos, TileBase tile,
            bool handleMulticell = true)
        {
            GetTilemap(layer).SetTile(pos, tile);
            if (handleMulticell) HandleMulticell(layer, pos, tile);
        }

        protected virtual void HandleMulticell(TLayer layer, Vector2Int pos, TileBase @override)
        {
            var existing = GetTile<MulticellDataTile>(layer, pos);
            if (existing == null) return;

            var parentPos = existing.ParentPosition;
            var mt = existing as MulticellTile;

            RectInt? shape = mt != null ? mt.Shape : null;
            var prefabInst = mt != null ? mt.PrefabReference : null;

            if (parentPos != null)
            {
                // prefer handling parent cell
                HandleMulticell(layer, (Vector2Int)parentPos.Value, @override);
                return;
            }

            if (shape != null)
            {
                // override but without checking
                foreach (var p in shape.Value.allPositionsWithin)
                    SetTile(layer, p, @override, handleMulticell: false);
            }

            if (prefabInst != null)
            {
                // multiblock is gone - kill inst
                this.SafeDestroy(prefabInst);
            }
        }

        public Dictionary<Vector2Int, TileBase> GetAllTiles(TLayer layer, bool notNull = false)
            => GetTilemap(layer).GetAllTiles(notNull);

        public Dictionary<Vector2Int, T> GetAllTilesOfType<T>(TLayer layer) where T : TileBase
            => GetTilemap(layer).GetAllTilesOfType<T>();

        public Dictionary<Vector2Int, TileBase> GetTilesInRange(TLayer layer, Vector2Int position,
            int range)
            => GetTilemap(layer).GetTilesInRange(position, range);

        public Dictionary<Vector2Int, TileBase> GetAdjacentTiles(TLayer layer, Vector2Int position,
            bool allowDiagonal)
            => GetTilemap(layer).GetAdjacentTiles(position, allowDiagonal);

        public Dictionary<Vector2Int, TileBase> GetConnectedTiles(TLayer layer, Vector2Int position,
            TileBase tile, bool allowDiagonal)
            => GetTilemap(layer).GetConnectedTiles(position, tile, allowDiagonal);

        #endregion

        public (TLayer layer, Vector2Int position, TileBase tile)? GetHighestTile(Vector2Int position)
        {
            var layers = GetAllTilesOn(position);
            if (layers == null || layers.Count == 0) return null;
            return layers.LastOrDefault(q => q.tile != null); // it's ordered by ascending
        }

        public (TLayer later, Vector2Int position, T tile)? GetHighestTile<T>(Vector2Int position)
            where T : TileBase
        {
            var all = GetAllTilesOn(position);
            if (all == null || all.Count == 0) return null;
            return ((TLayer later, Vector2Int position, T tile)?)all.LastOrDefault(q => q.tile is T);
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
