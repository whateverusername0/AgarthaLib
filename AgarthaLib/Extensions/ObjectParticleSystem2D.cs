using System;
using UnityEngine;

namespace AgarthaLib.MonoBehavior
{
    public class ObjectParticleSystem2D : MonoBehaviour
    {
        [Serializable] public class ObjectParticleConfiguration
        {
            /// <summary>
            ///     How many particles to emit each time?
            /// </summary>
            public int Amount = 1;

            /// <summary>
            ///    In which direction should the particles be emitted?
            /// </summary>
            public Vector2 Direction = Vector2.up;

            /// <summary>
            ///     How many emits per second to perform?
            /// </summary>
            public float Frequency = 1f;

            /// <summary>
            ///     How far should the particle fly?
            /// </summary>
            public float Distance = 1f;

            /// <summary>
            ///     In degrees, how wide should the spread be?
            /// </summary>
            [Range(1f, 360f)] public float Spread = 360f;

            /// <summary>
            ///     How long should the particle last?
            /// </summary>
            public float Lifetime = 1f;

            public ObjectParticleConfiguration(int amount, Vector2 direction)
            {
                Amount = amount;
                Direction = direction;
            }
        }

        public void EmitOnce(GameObject particle, ObjectParticleConfiguration config)
        {
            var go = new GameObject("[AUTOGEN] Partcile");
        }
    }
}