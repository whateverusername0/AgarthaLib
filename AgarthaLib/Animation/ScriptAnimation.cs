using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     Provides a basic ticker with customizable functions.
    /// </summary>
    public abstract class ScriptAnimation : AgarthanBehaviour
    {
        public bool Loop = false;
        public float Speed = 1f;
        public float Duration = 1f;
        private float _time = 0f;

        private void Update()
        {
            Tick();

            _time += Time.deltaTime * Speed;
            if (_time < Duration) return;

            if (Loop) _time = 0f;
            else Destroy(this.gameObject);
        }

        public abstract void Tick();
    }
}
