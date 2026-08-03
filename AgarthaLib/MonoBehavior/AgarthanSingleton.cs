using AgarthaLib.Data;
using AgarthaLib.Extensions;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    /// <summary>
    ///     Base class for everything you want to have a single examplar and direct access of.
    ///     Implements <see cref="AgarthanBehaviour"/>!
    /// </summary>
    public abstract class AgarthanSingleton<T> : AgarthanBehaviour where T : AgarthanSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                // find objects of type first
                _instance = _instance == null
                    ? FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).FirstOrDefault()
                    : _instance;

                // automatically create an instance jic
                _instance = _instance == null
                    ? new GameObject() { name = $"[Singleton] {typeof(T).Name}", hideFlags = HideFlags.DontSave }.EnsureComponent<T>()
                    : _instance;

                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null)
            {
                _instance = (T)this;
                return;
            }

            // instance exists.
            if (_instance is IDoNotDestroyOnLoad)
            {
                // it's probably important
                this.SafeDestroy(this);
                return;
            }

            // override
            this.SafeDestroy(_instance);
            _instance = (T)this;
        }
    }
}
