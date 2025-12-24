using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class UnityEngineExtensions
    {
        public static bool TryFindComponent<T>(this GameObject @object, out T comp) where T : Component
        {
            comp = @object.transform.GetComponentInChildren<T>() ?? null;
            if (comp == null)
            {
                if (@object.transform.parent != null)
                    return @object.transform.parent.gameObject.TryFindComponent(out comp);
                return false;
            }
            return true;
        }

        public static T EnsureComponent<T>(this GameObject @object) where T : Component
        {
            return @object.GetComponent<T>()
                ?? @object.AddComponent<T>();
        }

        public static bool TryGetComponentInChildren<T>(this GameObject @object, out T comp) where T : Component
        {
            comp = @object.transform.GetComponentInChildren<T>() ?? null;
            if (comp == null) return false;
            return true;
        }

        public static bool TryFind(this Transform t, string name, out Transform child)
            => (child = t.Find(name)) != null;

        public static List<Transform> GetChildren(this Transform t)
        {
            var l = new List<Transform>();
            for (int i = 0; i < t.childCount; i++)
                l.Add(t.GetChild(i));
            return l;
        }

        public static bool IsInLayerMask(this GameObject @object, LayerMask lm)
            => lm == (lm | (1 << @object.layer));
    }
}