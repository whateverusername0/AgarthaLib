using AgarthaLib.EventSystem;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib.ECS.Systems
{
    /// <summary>
    ///     Raises global events based on existing UnityEngine's static delegates.
    /// </summary>
    public class StaticEventDispatcher : EntitySystem
    {
        public override void Initialize()
        {
            // static event hate
            Tilemap.tilemapTileChanged += OnTilemapTileChanged;
            Tilemap.tilemapPositionsChanged += OnTilemapPositionsChanged;
        }

        public override void UpdateSystem()
        {
            // nuthing
        }

        private void OnTilemapTileChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
            => RaiseGlobalEvent(new TilemapTileChangedGlobalEvent(tilemap, tiles));

        private void OnTilemapPositionsChanged(Tilemap tilemap, NativeArray<Vector3Int> positions)
            => RaiseGlobalEvent(new TilemapPositionsChangedGlobalEvent(tilemap, positions));
    }

    public class TilemapTileChangedGlobalEvent : EventBase
    {
        public Tilemap Tilemap;
        public Tilemap.SyncTile[] Tiles;

        public TilemapTileChangedGlobalEvent(Tilemap tilemap, Tilemap.SyncTile[] tiles)
        {
            Tilemap = tilemap;
            Tiles = tiles;
        }
    }

    public class TilemapPositionsChangedGlobalEvent : EventBase
    {
        public Tilemap Tilemap;
        public NativeArray<Vector3Int> Positions;

        public TilemapPositionsChangedGlobalEvent(Tilemap tilemap, NativeArray<Vector3Int> positions)
        {
            Tilemap = tilemap;
            Positions = positions;
        }
    }
}
