using UnityEngine;

namespace Agartha.MonoBehavior
{
    public class ValueRangeRegenerating : ValueRange
    {
        [Header("Passive Recovery")]
        public float RegenRate = 5f;
        public float RegenCooldown = 1f;
        private float _regenCooldown = 0f;

        protected virtual void Update()
        {
            if (_regenCooldown <= 0)
            {
                var regen = RegenRate * Time.deltaTime;
                Value = Mathf.Clamp(Value + regen, 0, Max);
            }
            else _regenCooldown = Mathf.Clamp(_regenCooldown - Time.deltaTime, 0, RegenCooldown);
        }

        public virtual bool TryAdjustAmount(float amount)
        {
            var delta = Value + amount;
            if (delta < 0) return false;

            Value = Mathf.Clamp(delta, 0, Max);
            _regenCooldown = RegenCooldown;
            return true;
        }
    }
}