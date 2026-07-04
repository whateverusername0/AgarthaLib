using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Sprites.Effects
{
    public class TransformParallaxController : AgarthanBehaviour
    {
        public Camera Camera;

        [SerializeField, EditorReadOnly] private Vector3 _origin = Vector3.zero;
        [Range(0f, 1f)] public float Multiplier = 1f;

        protected override void Start()
        {
            base.Start();

            Camera = Camera == null ? Camera.main : Camera;

            _origin = transform.position;
        }

        protected override void Update()
        {
            base.Update();

            var pos = (Camera.transform.position - _origin) * (1f - Multiplier);
            transform.position = pos;
        }
    }
}
