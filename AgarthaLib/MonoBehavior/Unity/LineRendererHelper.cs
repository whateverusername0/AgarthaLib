using AgarthaLib.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.MonoBehavior.Unity
{
    [RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
    public class LineRendererHelper : AgarthanBehaviour
    {
        [ValidateNull] public LineRenderer LineRenderer;

        public List<Vector3> Points = new();
        public bool UseLocalSpace = true;

        protected override void Update()
        {
            base.Update();

            if (LineRenderer == null)
                return;

            for (int i = 0; i < Points.Count; i++)
                LineRenderer.SetPosition(i, Points[i] + (UseLocalSpace ? transform.position : Vector3.zero));
        }
    }
}
