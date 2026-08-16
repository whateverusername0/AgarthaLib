using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.ECS
{
    /// <summary>
    ///     A definition of a singleton meant to run in parallel with other singletons.
    ///     Has an EventQueue, which is supposed to be managed by an <see cref="EntitySystemManager"/>
    ///     in order to synchronize events across behaviors.
    /// </summary>
    /// <remarks>
    ///     Only supports raising noref global events
    ///     because there is no way you can access objects from over there.
    /// </remarks>
    public abstract class EntitySystem : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void UpdateSystem();

        #region Events

        public readonly List<object> EventQueue = new();

        public virtual void RaiseGlobalEvent<TArgs>(TArgs args) where TArgs : class
            => EventQueue.Add(args);

        #endregion
    }
}
