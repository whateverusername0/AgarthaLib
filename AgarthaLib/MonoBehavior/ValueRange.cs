using UnityEngine;

namespace Agartha.MonoBehavior
{
    public class ValueRange : MonoBehaviour
    {
        [SerializeField] protected float _value = 0f;
        public float Value
        {
            get { return _value; }
            protected set { _value = SetAmount(value); }
        }

        [SerializeField] protected float _max = 100f;
        public float Max
        {
            get { return _max; }
            protected set { _max = value; }
        }

        [SerializeField] protected float _min = 0f;
        public float Min
        {
            get { return _min; }
            protected set { _min = value; }
        }

        public float Normalized => (_value - _min) / (_max - _min);

        public virtual void AdjustAmount(float amount)
            => SetAmount(_value + amount);

        public virtual float SetAmount(float amount)
        {
            _value = Mathf.Clamp(amount, Min, Max);
            return _value;
        }
    }
}