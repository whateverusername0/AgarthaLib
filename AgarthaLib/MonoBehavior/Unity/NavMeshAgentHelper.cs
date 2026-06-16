using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace AgarthaLib.MonoBehavior.Unity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentHelper : AgarthanBehaviour
    {
        [ValidateNull] public NavMeshAgent Agent;

        [SerializeField, EditorReadOnly] private bool _hasPath = false;
        [SerializeField, EditorReadOnly] private NavMeshPathStatus _pathStatus;

        public bool HasPath => _hasPath;
        public NavMeshPathStatus PathStatus => _pathStatus;

        [Header("Editor")]
        [SerializeField] private bool _drawPath = true;
        [SerializeField] private Color _pathColor = Color.yellow;
        [SerializeField] private Color _nextPointColor = Color.red;

        protected override void Update()
        {
            base.Update();

            _hasPath = Agent.hasPath;
            _pathStatus = Agent.pathStatus;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_drawPath || Agent.path == null)
                return;

            Gizmos.color = _pathColor;
            var tr = Agent.transform;
            var corners = Agent.path.corners;
            for (int i = 0; i < corners.Length; i++)
            {
                var j = i + 1;
                if (j >= corners.Length - 1)
                    break;

                var apos = i == 0 ? tr.position : corners[i];
                Gizmos.DrawLine(apos, corners[j]);
            }

            Gizmos.color = _nextPointColor;
            Gizmos.DrawLine(tr.position, Agent.nextPosition);
        }
    }
}
