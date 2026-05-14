using AgarthaLib.Attributes;
using AgarthaLib.Data;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.UI
{
    public class UISpriteSlider : AgarthanBehaviour
    {
        public ValueRange<float> Thresholds;

        [Range(0f, 1f)] [SerializeField, EditorReadOnly] private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = Mathf.Clamp01(value); }
        }

        [ValidateNull] public RectTransform Rect;
        public bool UseLerp = false;
        public float LerpSpeed = 1f;
        public Vector3Constraint MovementConstraint;

        protected override void Update()
        {
            if (Rect == null)
                return;

            var t = MovementConstraint;
            var pos = Rect.localPosition;

            float targetPos = (Thresholds.Max + (Thresholds.Min * Value)).Reverse(Thresholds.Min);
            var vecPos = new Vector3(t.X ? targetPos : pos.x, t.Y ? targetPos : pos.y, t.Z ? targetPos : pos.z);

            Rect.localPosition = UseLerp ? Vector3.Lerp(pos, vecPos, Time.deltaTime * LerpSpeed) : vecPos;
        }
    }
}