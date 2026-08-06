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

            Origin = transform.position;
        }

        protected override void Update()
        {
            base.Update();

            var cpos = Camera.transform.position;
            var pos = (cpos - Origin) * (1f - Multiplier);
            transform.position = new(pos.x, pos.y, transform.position.z);
        }
    }
}
