using AgarthaLib.Attributes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AgarthaLib._2D.Tilemaps
{
    [ExecuteAlways, RequireComponent(typeof(TilemapCollider2D))]
    public class TilemapCollider3D : AgarthanBehaviour
    {
        [SerializeField, EditorReadOnly]
        private TilemapCollider2D _tilemapCollider;

        [SerializeField, EditorReadOnly]
        private MeshCollider _meshCollider;

        [SerializeField, EditorReadOnly]
        private Mesh _mesh;

        public float Depth = .5f;

        private void OnEnable()
        {
            _meshCollider = gameObject.EnsureChild("_meshCollider").EnsureComponent<MeshCollider>();
            _tilemapCollider = GetComponent<TilemapCollider2D>();
            _tilemapCollider.excludeLayers = 1;
            RegenerateMesh();
        }

        private void OnDisable()
        {
            _tilemapCollider.excludeLayers = 0;
        }

        protected override void Update()
        {
            base.Update();

            if (_meshCollider == null || _tilemapCollider == null)
                return;

            _meshCollider.convex = _tilemapCollider.isTrigger;
            _meshCollider.isTrigger = _tilemapCollider.isTrigger;
            _meshCollider.includeLayers = _tilemapCollider.includeLayers;
            _meshCollider.excludeLayers = _tilemapCollider.excludeLayers;

            if (_tilemapCollider.hasTilemapChanges)
            {
                _tilemapCollider.ProcessTilemapChanges();
                RegenerateMesh();
            }
        }

        [ContextMenu("Regenerate mesh")]
        private void RegenerateMesh()
        {
            if (_mesh != null) this.SafeDestroy(_mesh);
            _mesh = _tilemapCollider.CreateMesh(false, false);

            if (_mesh == null) return;
            _meshCollider.sharedMesh = _mesh;
            if (Depth <= 0f) return;

            var verts = _mesh.vertices;
            var tris = _mesh.triangles;

            var vertCount = verts.Length;
            var newVerts = new Vector3[vertCount * 2];

            float halfDepth = Depth * 0.5f;
            for (int i = 0; i < vertCount; i++)
            {
                newVerts[i] = new Vector3(verts[i].x, verts[i].y, -halfDepth);
                newVerts[i + vertCount] = new Vector3(verts[i].x, verts[i].y, halfDepth);
            }

            var newTris = new List<int>();

            for (int i = 0; i < tris.Length; i += 3)
            {
                newTris.Add(tris[i]);
                newTris.Add(tris[i + 1]);
                newTris.Add(tris[i + 2]);
            }

            for (int i = 0; i < tris.Length; i += 3)
            {
                newTris.Add(tris[i + 2] + vertCount);
                newTris.Add(tris[i + 1] + vertCount);
                newTris.Add(tris[i] + vertCount);
            }

            for (int i = 0; i < vertCount; i++)
            {
                int next = (i + 1) % vertCount;
                newTris.Add(i);
                newTris.Add(next);
                newTris.Add(i + vertCount);

                newTris.Add(next);
                newTris.Add(next + vertCount);
                newTris.Add(i + vertCount);
            }

            var extruded = new Mesh
            {
                vertices = newVerts,
                triangles = newTris.ToArray()
            };

            extruded.RecalculateNormals();
            extruded.RecalculateBounds();
            extruded.Optimize();

            _mesh = extruded;
            _meshCollider.sharedMesh = _mesh;
        }
    }
}
