using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class TimedDespawn : AgarthanBehaviour
    {
        public GameObject BoundObject;
        public float Lifetime = 1f;
        public GameObject SpawnOnDespawn;

        protected override void Start()
        {
            base.Start();

            BoundObject = BoundObject == null ? gameObject : BoundObject;
            RegisterTimer(Lifetime, () =>
            {
                if (SpawnOnDespawn != null)
                    Instantiate(SpawnOnDespawn, BoundObject.transform.position, Quaternion.identity);
                Destroy(BoundObject);
            });
        }

        public static void Add(GameObject boundObject, float lifetime)
        {
            var td = boundObject.AddComponent<TimedDespawn>();
            td.BoundObject = boundObject;
            td.Lifetime = lifetime;
        }
    }
}