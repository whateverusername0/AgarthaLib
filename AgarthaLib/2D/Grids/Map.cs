using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Grids
{
    public abstract class Map<TGrid, TGridLayer, TLayer> : AgarthanBehaviour
        where TGrid : MapGrid<TGridLayer, TLayer>
        where TGridLayer : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        public List<TGrid> Grids = new();

        public bool UseGlobalGrid = false;
        public TGrid GlobalGrid;

        public float ZPosition => transform.position.z;

        public virtual TGrid GetOverlappingGrid(Vector2 pos)
        {
            var olc = Physics2D.OverlapCircleAll(pos, .5f)
                .Select(q => q.GetComponent<TGridLayer>());
            if (olc.Count() == 0) return null;

            return olc.First().GetGrid() as TGrid;
        }

        public virtual TGrid CreateGrid(Vector3 pos, Quaternion rot, string name = "", bool isStatic = false)
        {
            name = string.IsNullOrWhiteSpace(name) ? Guid.NewGuid().ToString() : name;
            var go = transform.EnsureChild($"grid_{name}");
            go.transform.SetPositionAndRotation(pos, rot);

            var gd = go.EnsureComponent<TGrid>();
            gd.ResolveLayers();

            ResolveGrids();

            gd.IsStatic = isStatic;
            if (!isStatic)
            {
                // let that sink in
                var rb = go.EnsureComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.constraints =
                    RigidbodyConstraints.FreezePositionZ
                    & RigidbodyConstraints.FreezeRotationX
                    & RigidbodyConstraints.FreezeRotationY;
            }

            return gd;
        }

        public virtual TileBase GetTile(TLayer layer, Vector2Int pos)
        {
            var grid = GetOverlappingGrid(pos);
            grid = grid == null && UseGlobalGrid ? GlobalGrid : grid;
            if (grid == null) return null;

            return grid.GetTile(layer, pos);
        }

        public virtual void SetTile(TLayer layer, Vector2Int pos, TileBase tile)
        {
            var grid = GetOverlappingGrid(pos);
            grid = grid == null && UseGlobalGrid ? GlobalGrid : grid;
            grid = grid == null ? CreateGrid((Vector3Int)pos, GlobalGrid.transform.rotation) : grid;

            grid.SetTile(layer, pos, tile);
        }

        public virtual List<TGrid> ResolveGrids()
        {
            Grids = GetComponentsInChildren<TGrid>()
                .Where(q => UseGlobalGrid || q != GlobalGrid)
                .ToList();

            return Grids;
        }

        public abstract void MakeActive();
    }
}
