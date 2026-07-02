using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using SALVAGE.Input;
using UnityEngine;

namespace AgarthaLib.Sprites.Effects
{
    public class SpriteParallaxController : AgarthanBehaviour
    {
        private CameraManager _cam => CameraManager.Instance;

        [SerializeField, EditorReadOnly] private Vector3 _origin = Vector3.zero;
        [Range(0f, 1f)] public float Multiplier = 1f;

        protected override void Start()
        {
            base.Start();

            _origin = transform.position;
        }

        protected override void Update()
        {
            base.Update();

            var pos = (_cam.transform.position - _origin) * (1f - Multiplier);
            transform.position = pos;
        }
    }
}
