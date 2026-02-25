using AgarthaLib.Attributes;
using AgarthaLib.EventSystem;
using AgarthaLib.EventSystem.EventBus;
using AgarthaLib.Extensions;
using AgarthaLib.Timing;
using System;
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
        public void RaiseEvent<TArgs>(GameObject target, TArgs args) where TArgs : class
            => RaiseEvent(gameObject, target, args);

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject invoker, GameObject target, TArgs args) where TArgs : class
            => _bus.RaiseEvent(invoker, target, args);

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject target, ref TArgs args) where TArgs : class
            => RaiseEvent(gameObject, target, ref args);

        /// <inheritdoc/>
        public void RaiseEvent<TArgs>(GameObject invoker, GameObject target, ref TArgs args) where TArgs : class
            => _bus.RaiseEvent(invoker, target, ref args);

        /// <inheritdoc/>
        public void SubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.SubscribeEvent(handler);

        /// <inheritdoc/>
        public void UnsubscribeEvent<TArgs>(LocalEventHandlerDelegate<TArgs> handler) where TArgs : class
            => _bus.UnsubscribeEvent(handler);

        #endregion

        #region GlobalEventBus

        private GlobalEventBus _globalBus => GlobalEventBus.Instance;

        public void RaiseGlobalEvent<TArgs>(TArgs args) where TArgs : class
            => _globalBus.RaiseEvent(args);

        public void RaiseGlobalEvent<TArgs>(ref TArgs args) where TArgs : class
            => _globalBus.RaiseEvent(ref args);

        public void SubscribeGlobalEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _globalBus.SubscribeEvent(handler);

        public void UnsubscribeGlobalEvent<TArgs>(EventHandlerDelegate<TArgs> handler) where TArgs : class
            => _globalBus.UnsubscribeEvent(handler);

        #endregion

        #region Event Relays

        /// <summary>
        ///     Relays the event to specific targets.
        ///     Then replaces the original event results with itself.
        /// </summary>
        /// <param name="targets">Targets to relay the event to.</param>
        /// <param name="args">The event.</param>
        public void RelayEvent<TArgs>(List<GameObject> targets, ref TArgs args) where TArgs : class
        {
            var relay = new RelayedEvent<TArgs>(gameObject, args);
            foreach (var target in targets)
                RaiseEvent(target, ref relay);
            args = relay.Args;
        }

        // todo change to include the entire hierarchy
        public void RaiseRelayEvent<TArgs>(ref TArgs args) where TArgs : class
        {
            RaiseEvent(gameObject, ref args);

            var children = transform.GetChildren().Select(q => q.gameObject).ToList();
            RelayEvent(children, ref args);
        }

        #endregion

        #region Timers

        private readonly List<TimerInstance> _timers = new();

        protected void RegisterTimer(float delay, Action a, bool overwrite = false)
        {
            var existing = GetTimer(a);
            if (existing != null)
            {
                if (overwrite) UnregisterTimer(existing);
                else throw new ArgumentException("Action");
            }

            _timers.Add(new(delay, a));
        }

        protected void UnregisterTimer(TimerInstance inst)
            => _timers.Remove(inst);

        protected void UnregisterTimer(Action a)
        {
            var existing = GetTimer(a);
            if (existing != null) UnregisterTimer(existing);
        }

        protected TimerInstance GetTimer(Action a)
        {
            var existing = _timers.Where(q => q.Action == a).FirstOrDefault();
            if (existing != null) return existing;
            return null;
        }

        #endregion

        protected virtual void Start()
        {
            ValidateNull();
        }

        protected virtual void Update()
        {
            UpdateTimers();
        }

        #region Helper Methods

        private void ValidateNull()
        {
            var fields = GetType().GetFields();
            foreach (var item in fields.Where(q => Attribute.IsDefined(q, typeof(ValidateNullAttribute))))
            {
                if (item.FieldType.IsSubclassOf(typeof(Component)))
                {
                    var value = item.GetValue(this) as Component;
                    item.SetValue(this, value == null ? GetComponent(item.FieldType) : value);
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