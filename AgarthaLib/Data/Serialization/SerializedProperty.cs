using System;
using UnityEngine;

[Serializable] public abstract class SerializedProperty<T> : MonoBehaviour
{
    [SerializeField] protected T _value;
    public virtual T Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public static implicit operator T(SerializedProperty<T> @this)
        => @this.Value;

    public T Deserialize(string json)
        => JsonUtility.FromJson<T>(json);

    public string Serialize()
        => JsonUtility.ToJson(this);
}
