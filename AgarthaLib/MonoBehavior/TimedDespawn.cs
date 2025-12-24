using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class TimedDespawn : MonoBehaviour
    {
        public GameObject BoundObject;
        public float Lifetime = 1f;
        private float LifetimeTimer = 1f;

        private void Start()
        {
            BoundObject = BoundObject == null ? gameObject : BoundObject;
            LifetimeTimer = Lifetime;
        }

        private void Update()
        {
            LifetimeTimer -= Time.deltaTime;
            if (LifetimeTimer <= 0)
            {
                if (BoundObject != null) Destroy(BoundObject);
            }
        }

        public static void Add(GameObject boundObject, float lifetime)
        {
            var td = boundObject.AddComponent<TimedDespawn>();
            td.BoundObject = boundObject;
            td.Lifetime = lifetime;
        }
    }
}