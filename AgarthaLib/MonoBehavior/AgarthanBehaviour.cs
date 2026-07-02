using AgarthaLib.Attributes;
using AgarthaLib.EventSystem;
using AgarthaLib.EventSystem.EventBus;
using AgarthaLib.Extensions;
using AgarthaLib.Timing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    /// <summary>
    ///     Base class for all <see cref="MonoBehaviour"/> objects that want to use AgarthaLib's functionality.
    ///     Sure you can manually implement everything, but you'd be better off inheriting this bad boy.
    /// </summary>
    public abstract class AgarthanBehaviour : MonoBehaviour, ILocalEventBus
    {
        #region ILocalEventBus

        private readonly LocalEventBus _bus = new();

        /// <inheritdoc cref="LocalEventBus.GetSubscriptions"/>
        public Dictionary<Type, Delegate> GetSubscriptions()
            => _bus.GetSubscriptions();

        /// <inheritdoc/>
        public virtual void RaiseEvent<TArgs>(TArgs args) where TArgs : class
            => RaiseEvent(gameObject, gameObject, args);

        /// <inheritdoc/>
        public virtual void RaiseEvent<TArgs>(GameObject target, TArgs args) where TArgs : class
            => RaiseEvent(gameObject, target, args);

        /// <inheritdoc/>
        public virtual void RaiseEvent<TArgs>(GameObject invoker, GameObject target, TArgs args) where TArgs : class
            => _bus.RaiseEvent(invoker, target, args);

        /// <inheritdoc/>
        public virtual void RaiseEvent<TArgs>(GameObject target, ref TArgs args) where TArgs : class
            => RaiseEvent(gameObject, target, ref args);

        /// <inheritdoc/>
        public virtual void RaiseEvent<TArgs>(GameObject invoker, GameObject target, ref TArgs args) where TArgs : class
            => _bus.RaiseEvent(invoker, target, ref args);

        /// <inheritdoc/>
        public virtual void SubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.SubscribeEvent(handler);

        /// <inheritdoc/>
        public virtual void UnsubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.UnsubscribeEvent(handler);

        #endregion

        #region GlobalEventBus

        private GlobalEventBus _globalBus => GlobalEventBus.Instance;

        public virtual void RaiseGlobalEvent<TArgs>(TArgs args) where TArgs : class
            => _globalBus.RaiseEvent(args);

        public virtual void RaiseGlobalEvent<TArgs>(ref TArgs args) where TArgs : class
            => _globalBus.RaiseEvent(ref args);

        public virtual void SubscribeGlobalEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _globalBus.SubscribeEvent(handler);

        public virtual void UnsubscribeGlobalEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _globalBus.UnsubscribeEvent(handler);

        #endregion

        #region Event Relays

        public virtual void RelayEvent<TArgs>(GameObject target, ref TArgs args) where TArgs : class
        {
            var relay = new RelayedEvent<TArgs>(gameObject, args);
            RaiseEvent(target, ref relay);
            args = relay.Args;
        }

        public virtual void RelayEvent<TArgs>(List<GameObject> targets, ref TArgs args) where TArgs : class
        {
            foreach (var target in targets)
                RelayEvent(target, ref args);
        }

        // todo change to include the entire hierarchy
        public virtual void RaiseRelayEvent<TArgs>(ref TArgs args) where TArgs : class
        {
            RaiseEvent(gameObject, ref args);

            var children = transform.GetChildren().Select(q => q.gameObject).ToList();
            RelayEvent(children, ref args);
        }

        #endregion

        #region Timers

        private readonly List<TimerInstance> _timers = new();

        protected virtual void RegisterTimer(float delay, Action a, bool overwrite = false)
        {
            var existing = GetTimer(a);
            if (existing != null)
            {
                if (overwrite) UnregisterTimer(existing);
                else throw new ArgumentException("Action");
            }

            _timers.Add(new(delay, a));
        }

        protected virtual void UnregisterTimer(TimerInstance inst)
            => _timers.Remove(inst);

        protected virtual void UnregisterTimer(Action a)
        {
            var existing = GetTimer(a);
            if (existing != null) UnregisterTimer(existing);
        }

        protected virtual TimerInstance GetTimer(Action a)
        {
            var existing = _timers.Where(q => q.Action == a).FirstOrDefault();
            if (existing != null) return existing;
            return null;
        }

        #endregion

        protected virtual void Start()
        {
            ValidateNull();

            // add LateFixedUpdate();
            StartCoroutine(LateFixedUpdateEnumerator());
        }

        protected virtual void Update()
        {
            //UpdateTimers(); // todo refactor or remove
        }

        private IEnumerator LateFixedUpdateEnumerator()
        {
            var wffu = new WaitForFixedUpdate();
            while (this != null)
            {
                yield return wffu;
                try { LateFixedUpdate(); }
                // fault tolerance
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        protected virtual void LateFixedUpdate()
        {

        }

        #region Helper Methods

        private void ValidateNull()
        {
            var fields = GetType().GetFields();
            foreach (var item in fields.Where(q => Attribute.IsDefined(q, typeof(ValidateNullAttribute))))
            {
                var type = item.FieldType;
                var att = (ValidateNullAttribute)Attribute.GetCustomAttribute(item, typeof(ValidateNullAttribute));
                if (item.FieldType.IsSubclassOf(typeof(Component)))
                {
                    var fallback = att.Traverse ? GetComponentInChildren(type) : GetComponent(type);
                    var value = item.GetValue(this) as Component;
                    item.SetValue(this, value != null ? value : fallback);
                }
            }
        }

        private void UpdateTimers()
        {
            if (_timers.Count == 0) return;
            foreach (var timer in _timers)
            {
                timer.Timer -= Time.deltaTime;
                if (timer.Timer <= 0)
                {
                    timer.Timer = timer.Delay;
                    timer.Action.Invoke();
                }
            }
        }

        #endregion
    }
}