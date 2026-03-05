using UnityEngine;

namespace AgarthaLib.Data.Serialization.Behaviours
{
    public abstract class SerializedPropertyBridge<T, Q> : MonoBehaviour where T : SerializedProperty<Q> where Q : struct
    {
        public T Property;

        public Q Value => Property.Value;

        public static explicit operator SerializedProperty<Q>(SerializedPropertyBridge<T, Q> @this)
            => @this.Property;
    }
}
