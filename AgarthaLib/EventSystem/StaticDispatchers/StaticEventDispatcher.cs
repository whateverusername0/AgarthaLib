using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.EventSystem.StaticDispatchers
{
    public class StaticEventDispatcher : AgarthanSingleton<StaticEventDispatcher>
    {
        protected override void Start()
        {
            base.Start();

            // static event hate
            Tilemap.tilemapTileChanged += OnTilemapTileChanged;
            Tilemap.tilemapPositionsChanged += OnTilemapPositionsChanged;
        }

        private void OnTilemapTileChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
            => RaiseGlobalEvent(new TilemapTileChangedGlobalEvent(tilemap, tiles.ToList()));

        private void OnTilemapPositionsChanged(Tilemap tilemap, NativeArray<Vector3Int> positions)
            => RaiseGlobalEvent(new TilemapPositionsChangedGlobalEvent(tilemap, positions.ToList()));
    }

    public class TilemapTileChangedGlobalEvent
    {
        public Tilemap Tilemap;
        public List<Tilemap.SyncTile> Tiles;

        public TilemapTileChangedGlobalEvent(Tilemap tilemap, List<Tilemap.SyncTile> tiles)
        {
            Tilemap = tilemap;
            Tiles = tiles;
        }
    }

    public class TilemapPositionsChangedGlobalEvent
    {
        public Tilemap Tilemap;
        public List<Vector3Int> Positions;

        public TilemapPositionsChangedGlobalEvent(Tilemap tilemap, List<Vector3Int> positions)
        {
            Tilemap = tilemap;
            Positions = positions;
        }
    }
}
