using UnityEngine;

namespace AgarthaLib.Data.Serialization.Behaviours
{
    public abstract class SerializedPropertyBridge<T> : MonoBehaviour where T : struct
    {
        public SerializedProperty<T> Property;

        public static explicit operator SerializedProperty<T>(SerializedPropertyBridge<T> @this)
            => @this.Property;
    }
}
