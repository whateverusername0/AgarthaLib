using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class UnityEngineExtensions
    {
        public static bool HasComponent<T>(this GameObject @object) where T : Component
            => @object.GetComponent<T>() != null;

        public static bool HasComponent<T>(this Component @object) where T : Component
            => @object.GetComponent<T>() != null;

        public static bool TryGetComponentInChildren<T>(this GameObject @object, out T comp) where T : Component
        {
            comp = @object.transform.GetComponentInChildren<T>() ?? null;
            if (comp == null) return false;
            return true;
        }

        public static bool TryGetComponentInChildren<T>(this Component @object, out T comp) where T : Component
            => TryGetComponentInChildren(@object.gameObject, out comp);

        public static bool TryGetComponentInTree<T>(this GameObject @object, out T comp) where T : Component
        {
            if (!@object.gameObject.TryGetComponentInChildren<T>(out comp))
            {
                if (@object.transform.parent != null)
                    return @object.transform.parent.TryGetComponentInTree(out comp);
                return false;
            }
            return true;
        }

        public static bool TryGetComponentInTree<T>(this Component @object, out T comp) where T : Component
            => TryGetComponentInTree(@object.gameObject, out comp);

        public static T EnsureComponent<T>(this GameObject @object) where T : Component
            => @object.GetComponent<T>() ?? @object.AddComponent<T>();

        public static T EnsureComponent<T>(this Component @object) where T : Component
            => @object.gameObject.EnsureComponent<T>();

        public static bool TryFindChild(this Transform t, string name, out Transform child)
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