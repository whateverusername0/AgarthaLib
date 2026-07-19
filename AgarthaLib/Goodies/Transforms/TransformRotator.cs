using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Transforms
{
    public class TransformRotator : AgarthanBehaviour
    {
        public Vector3 Speed = Vector3.down;

        [Header("Sine")]
        public bool Sine = false;
        public float SineOrigin = 0f;
        public float SineSpeed = 1f;
        [SerializeField, EditorReadOnly] private Vector3 _origin = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            _origin = transform.localPosition;
        }

        protected override void Update()
        {
            base.Update();
            var sin = Mathf.Sin(Time.time + SineOrigin);
            var delta = (Sine ? sin * SineSpeed : Time.time) * Speed;
            var localpos = transform.localEulerAngles;
            localpos.x = Speed.x != 0 ? delta.x : localpos.x;
            localpos.y = Speed.y != 0 ? delta.y : localpos.y;
            localpos.z = Speed.z != 0 ? delta.z : localpos.z;
            transform.localEulerAngles = _origin + localpos;
        }
    }
}
