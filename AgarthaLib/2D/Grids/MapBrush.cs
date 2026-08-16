using System;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    public abstract class MapBrush<TMap, TGrid, TGridLayer, TLayer> : GridBrushBase
        where TMap : Map<TGrid, TGridLayer, TLayer>
        where TGrid : MapGrid<TGridLayer, TLayer>
        where TGridLayer : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        public TGrid ActiveGrid;
        public TLayer ActiveLayer;

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            if (ActiveGrid == null) return;

            var tilemap = ActiveGrid.GetTilemap(ActiveLayer);
            if (tilemap == null) return;

            brushTarget = tilemap.gameObject;
            if (brushTarget == null) return;

            position.z = (int)(object)ActiveLayer;
            base.Paint(gridLayout, brushTarget, position);
        }
    }
}
