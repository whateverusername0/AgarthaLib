using AgarthaLib.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.MonoBehavior.Unity
{
    [RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
    public class LineRendererHelper : AgarthanBehaviour
    {
        [ValidateNull] public LineRenderer LineRenderer;

        [Tooltip("Overrides points.")]
        public List<Transform> Links = new();

        public List<Vector3> Points = new();
        public bool UseLocalSpace = true;

        protected override void Update()
        {
            base.Update();

            if (LineRenderer == null)
                return;

            if (Links != null && Links.Count > 0)
                UpdateLinks();

            else if (Points != null && Points.Count > 0)
                UpdatePoints();
        }

        private void UpdateLinks()
        {
            foreach (var t in Links)
            {
                if (t == null) continue;
                LineRenderer.SetPosition(Links.IndexOf(t), t.position);
            }
        }

        private void UpdatePoints()
        {
            for (int i = 0; i < Points.Count; i++)
                LineRenderer.SetPosition(i, Points[i] + (UseLocalSpace ? transform.position : Vector3.zero));
        }
    }
}
