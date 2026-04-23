using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System.Collections;
using UnityEngine;

namespace AgarthaLib.Collision.Handlers
{
    public class CollisionEventLink : AgarthanBehaviour
    {
        // TODO FIX EVENT RELAYING WHEN IT SHOULDNT

        // i'll just assume it's never null
        public CollisionEventLink Link;
        [EditorReadOnly] public int Contacts = 0;

        public bool CollidingLocal => Contacts > 0;
        public bool CollidingGlobal => CollidingLocal || Link.CollidingLocal;

        [Header("Editor")]
        [SerializeField, EditorReadOnly] private bool _collidingLocal;
        [SerializeField, EditorReadOnly] private bool _collidingGlobal;

        protected override void Start()
        {
            base.Start();

            #region Relay

            SubscribeEvent<CollisionEnterEvent>(LocalCollisionEnter);
            SubscribeEvent<Collision2DEnterEvent>(LocalCollisionEnter);

            SubscribeEvent<CollisionExitEvent>(LocalCollisionExit);
            SubscribeEvent<Collision2DExitEvent>(LocalCollisionExit);

            SubscribeEvent<AfterCollisionEnterEvent>(Relay);
            SubscribeEvent<AfterCollision2DEnterEvent>(Relay);
            SubscribeEvent<CollisionStayEvent>(Relay);
            SubscribeEvent<Collision2DStayEvent>(Relay);
            SubscribeEvent<AfterCollisionExitEvent>(Relay);
            SubscribeEvent<AfterCollision2DExitEvent>(Relay);

            #endregion
        }

        protected override void Update()
        {
            base.Update();

            _collidingLocal = CollidingLocal;
            _collidingGlobal = CollidingGlobal;
        }

        public void LocalCollisionEnter<T>(GameObject invoker, ref T args) where T : class
        {
            var contacts = Mathf.Max(Contacts + 1, 0);

            if (CollidingGlobal)
            {
                Contacts = contacts;
                return;
            }

            Contacts = contacts;
            Relay(invoker, ref args);
        }

        public void LocalCollisionExit<T>(GameObject invoker, ref T args) where T : class
        {
            var contacts = Mathf.Max(Contacts - 1, 0);
            Contacts = contacts;

            // run the detection at the end of frame in case player has entered another collision mid shit
            StartCoroutine(IELocalCollisionExit(invoker, args));
        }

        private IEnumerator IELocalCollisionExit<T>(GameObject invoker, T args) where T : class
        {
            var wfe = new WaitForEndOfFrame();
            for (int i = 0; i < 2; i++)
                yield return wfe;

            if (!CollidingGlobal)
                Relay(invoker, ref args);
        }

        public void Relay<T>(GameObject i, ref T a) where T : class
        {
            if (Link != null) RelayEvent(Link.gameObject, ref a);
        }
    }
}