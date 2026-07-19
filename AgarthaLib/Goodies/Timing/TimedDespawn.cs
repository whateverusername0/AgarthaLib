using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using UnityEngine;

namespace AgarthaLib.Goodies.Timing
{
    public class TimedDespawn : AgarthanBehaviour
    {
        public GameObject BoundObject;
        public GameObject SpawnOnDespawn;
        public float Lifetime = 1f;
        public float LifetimeTimer = 0f;

        protected override void Update()
        {
            base.Update();

            if (LifetimeTimer >= Lifetime)
            {
                if (SpawnOnDespawn != null)
                    Instantiate(SpawnOnDespawn, BoundObject.transform.position, Quaternion.identity);
                this.SafeDestroy(BoundObject);
                return;
            }

            LifetimeTimer += Time.deltaTime;
        }

        public static void Trigger(GameObject boundObject, float lifetime)
        {
            var td = boundObject.AddComponent<TimedDespawn>();
            td.BoundObject = boundObject;
            td.Lifetime = lifetime;
        }
    }
}