using AgarthaLib.Attributes;
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

        [SerializeField, EditorReadOnly] private bool _renderingEnabled = true;

        public virtual TGrid GetOverlappingGrid(Vector2 pos)
        {
            var olc = Physics2D.OverlapCircleAll(pos, .5f)
                .Select(q => q.GetComponent<TGridLayer>());
            if (olc.Count() == 0) return null;

            return olc.First().GetGrid() as TGrid;
        }

        public virtual TGrid CreateGrid(Vector3 pos, Quaternion rot, bool isStatic = false)
        {
            var go = new GameObject($"grid_{Guid.NewGuid()}");
            go.transform.SetParent(this.transform);
            go.transform.SetPositionAndRotation(pos, rot);
            var gd = go.EnsureComponent<TGrid>();
            gd.ResolveLayers();

            ResolveGrids();

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
            Grids = GetComponentsInChildren<TGrid>().Where(q => q != GlobalGrid).ToList();
            return Grids;
        }

        public abstract void MakeActive();

        public virtual void EnableRendering()
        {
            if (_renderingEnabled) return;

            _renderingEnabled = true;

            // hi hello. EVERYTHING in the hierarchy of MapData is MANAGED
            foreach (var renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = true;
        }

        public virtual void DisableRendering()
        {
            if (!_renderingEnabled) return;

            _renderingEnabled = false;

            // hi hello. EVERYTHING in the hierarchy of MapData is MANAGED
            foreach (var renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = false;
        }
    }
}
