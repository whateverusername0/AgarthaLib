using AgarthaLib.Attributes;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.ECS.Systems;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Grids
{
    // oh my fucking god
    // at least the implementation is as stupid as wood bark
    public abstract class MapManager<TMap, TGrid, TGridLayer, TLayer>
        : AgarthanSingleton<MapManager<TMap, TGrid, TGridLayer, TLayer>>
        where TMap : Map<TGrid, TGridLayer, TLayer>
        where TGrid : MapGrid<TGridLayer, TLayer>
        where TGridLayer : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        #region Maps

        [SerializeField, EditorReadOnly] protected List<TMap> _maps = new();
        [SerializeField, EditorReadOnly] protected TMap _activeMap;

        public List<TMap> Maps => _maps;
        public TMap ActiveMap => _activeMap;

        public virtual List<TMap> ResolveMaps()
            => _maps = FindObjectsOfType<TMap>(includeInactive: true).ToList();

        public virtual TMap ResolveActiveMap()
        {
            if (_maps == null || _maps.Count == 0)
                ResolveMaps();

            var map = _maps.FirstOrDefault();
            SetActiveMap(map);
            return map;
        }

        public virtual void SetActiveMap(TMap map)
        {
            if (map == null) return;

            var lastMap = _activeMap;
            _activeMap = map;
            ActiveMapChanged(lastMap, map);
        }

        public abstract void ActiveMapChanged(TMap lastMap, TMap newMap);

        public TMap AddMap(string name = "")
        {
            name = string.IsNullOrWhiteSpace(name) ? Guid.NewGuid().ToString() : name;
            var map = this.transform.EnsureChild($"map_{name}").EnsureComponent<TMap>();
            Maps.Add(map);
            return map;
        }

        #endregion

        #region Query

        public SerializedDictionary<TGridLayer, SerializedDictionary<Vector2Int, TileBase>>
            TileQuery = new();

        protected virtual void OnTilemapTileChanged(ref TilemapTileChangedGlobalEvent args)
            => ProcessQueryChange(args.Tilemap, args.Tiles);

        protected virtual void DirtyTileQuery()
        {
            // hoooooooly fucking shit
            TileQuery.Clear();
            foreach (var map in ResolveMaps())
                foreach (var grid in map.ResolveGrids())
                    foreach (var layer in grid.ResolveLayers())
                        TileQuery.Add(layer, new(layer.GetAllTiles()));
        }

        protected virtual void ProcessQueryChange(Tilemap t, Tilemap.SyncTile[] st)
        {
            if (t == null || !t.TryGetComponent<TGridLayer>(out var gridLayer))
                return;

            if (TileQuery.Count == 0)
                DirtyTileQuery(); // i pray this never happens

            foreach (var tile in st)
            {
                if (!TileQuery.ContainsKey(gridLayer))
                    TileQuery.Add(gridLayer, new());

                var position = (Vector2Int)tile.position;
                var exists = TileQuery[gridLayer].ContainsKey(position);

                if (tile.tile == null && !exists)
                    continue;

                if (tile.tile == null && exists)
                {
                    TileQuery[gridLayer].Remove(position);
                    continue;
                }

                if (tile.tile != null && !exists)
                    TileQuery[gridLayer].Add(position, tile.tile);

                if (tile.tile != null && exists)
                {
                    TileQuery[gridLayer][position] = tile.tile;
                    continue;
                }
            }
        }

        public virtual List<(TGridLayer gridLayer, Vector2Int position, T tile)> QueryTilesOfType<T>()
            where T : TileBase
        {
            if (TileQuery.Count == 0)
                DirtyTileQuery();

            var flat = GetQueryFlattened();
            var r = flat.Where(q => q.tile is T).ToList()
                .ConvertAll(q => (q.layer, q.pos, q.tile as T));

            return r;
        }

        public virtual IEnumerable<(TGridLayer layer, Vector2Int pos, TileBase tile)> GetQueryFlattened()
            => TileQuery.SelectMany(
                layer => layer.Value,
                (layerKvp, tileKvp) => (layerKvp.Key, tileKvp.Key, tileKvp.Value)
            );

        #endregion

        protected override void Start()
        {
            base.Start();

            DirtyTileQuery();
            SubscribeGlobalEvent<TilemapTileChangedGlobalEvent>(OnTilemapTileChanged);
        }
    }
}
