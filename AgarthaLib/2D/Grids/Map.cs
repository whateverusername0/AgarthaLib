using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D.Grids
{
    [RequireComponent(typeof(Grid))]
    public abstract class Map<TGrid, TGridLayer, TLayer> : AgarthanBehaviour
        where TGrid : MapGrid<TGridLayer, TLayer>
        where TGridLayer : MapGridLayer<TLayer>
        where TLayer : Enum
    {
        public List<TGrid> Grids = new();

        public bool UseGlobalGrid = false;
        public TGrid GlobalGrid;

        protected virtual Grid _grid => GetComponent<Grid>();
        public float ZPosition => transform.position.z;

        public virtual TGridLayer GetOverlappingGridLayer(Vector2 pos, float radius = 0f, int layerMask = -1)
        {
            var z = transform.position.z;
            var olc = radius <= 0f
                ? Physics2D.OverlapPointAll(pos, layerMask, -z + .1f, z - .1f)
                : Physics2D.OverlapCircleAll(pos, radius, layerMask, -z + .1f, z - .1f);

            if (olc.Length == 0) return null;
            return olc[0].GetComponent<TGridLayer>();
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

        public virtual List<TGrid> ResolveGrids()
        {
            Grids = GetComponentsInChildren<TGrid>()
                .Where(q => UseGlobalGrid || q != GlobalGrid)
                .ToList();

            return Grids;
        }

        public abstract void MakeActive();

        public virtual Vector3 ToWorldPosition(Vector2Int pos)
            => _grid.GetCellCenterWorld(new(pos.x, pos.y, Mathf.FloorToInt(ZPosition)));
    }
}
