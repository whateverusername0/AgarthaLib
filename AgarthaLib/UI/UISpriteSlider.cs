using AgarthaLib.Data;
using AgarthaLib.Extensions;
using UnityEngine;

namespace AgarthaLib.UI
{
    public class UISpriteSlider : MonoBehaviour
    {
        public MinMax Thresholds;

        [Range(0f, 1f)] [SerializeField] private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = Mathf.Clamp01(value); }
        }

        public bool UseLerp = false;
        public float LerpSpeed = 1f;
        public Vector3Constraint MovementConstraint;

        private void Update()
        {
            var @transform = GetComponent<RectTransform>();
            var t = MovementConstraint;
            var pos = @transform.localPosition;

            var targetPos = (Thresholds.Max + (Thresholds.Min * Value)).Reverse(Thresholds.Min);
            var vecPos = new Vector3(t.X ? targetPos : pos.x, t.Y ? targetPos : pos.y, t.Z ? targetPos : pos.z);

            transform.localPosition = UseLerp ? Vector3.Lerp(pos, vecPos, Time.deltaTime * LerpSpeed) : vecPos;
        }
    }
}