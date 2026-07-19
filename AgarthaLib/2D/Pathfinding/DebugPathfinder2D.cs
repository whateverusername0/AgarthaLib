using AgarthaLib.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D.Pathfinding
{
    public class DebugPathfinder2D : Pathfinder2D
    {
        [EditorReadOnly] public List<Vector3> Path = new();
        public bool TruncatePath = false;

        public Vector2Int StartPosition, EndPosition;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)StartPosition, .25f);
            Gizmos.DrawWireSphere((Vector2)EndPosition, .25f);

            if (Path != null && Path.Count > 0)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < Path.Count; i++)
                {
                    var pos = Path[i];
                    Gizmos.DrawWireSphere(pos, .25f);

                    if (i == 0) continue;
                    Gizmos.DrawLine(Path[i], Path[i - 1]);
                }
            }
        }

        [ContextMenu("Generate path")]
        public void GeneratePath()
        {
            if (TryFindPath(StartPosition, EndPosition, out var path))
            {
                if (TruncatePath) path = Pathfinding2D.Truncate(path);
                Path = path.Select(q => (Vector3)(Vector2)q).ToList();
            }
            else Debug.LogWarning($"Unable to find path to {EndPosition}");
        }
    }
}
