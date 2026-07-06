using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Transforms
{
    public class TransformParallaxController : AgarthanBehaviour
    {
        public Camera Camera;

        [SerializeField] public Vector3 Origin = Vector3.zero;
        [Range(0f, 1f)] public float Multiplier = 1f;

        protected override void Start()
        {
            base.Start();

            Camera = Camera == null ? Camera.main : Camera;

            Origin = transform.localPosition;
        }

        protected override void Update()
        {
            base.Update();

            var pos = (Camera.transform.position - Origin) * (1f - Multiplier);
            transform.localPosition = pos;
        }
    }
}
