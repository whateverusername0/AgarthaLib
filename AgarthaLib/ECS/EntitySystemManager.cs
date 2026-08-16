using AgarthaLib.Data;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;

namespace AgarthaLib.ECS
{
    /// <summary>
    ///     Manages entity systems.
    ///     Is supposed to be optimized for running parallel independent things.
    /// </summary>
    /// <remarks>
    ///     Put this in your scene and forget about it.
    ///     Sooner or later it will pay off.
    /// </remarks>
    public class EntitySystemManager : AgarthanSingleton<EntitySystemManager>, IDoNotDestroyOnLoad
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        protected override void Start()
        {
            base.Start();
            InitializeSystems();
        }

        protected override void Update()
        {
            base.Update();
            UpdateSystems();
        }

        public virtual void InitializeSystems()
        {
            foreach (var s in typeof(EntitySystem).GetAllDerivatives())
            {
                // TODO multithreading job unity system dots ecs etc etc bla bla
                var go = gameObject.EnsureChild(s.Name);
                var sc = go.EnsureComponent(s) as EntitySystem;
                sc.Initialize();
            }
        }

        public virtual void UpdateSystems()
        {
            // TODO multithreading job unity system dots ecs etc etc bla bla
            foreach (var s in GetComponentsInChildren<EntitySystem>())
            {
                // process data
                while (s.EventQueue.Count > 0)
                {
                    RaiseGlobalEvent(s.EventQueue[0]);
                    s.EventQueue.RemoveAt(0);
                }

                s.UpdateSystem();
            }
        }
    }
}
