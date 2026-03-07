using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Extensions
{
    public static class UnityEngineExtensions
    {
        #region Components

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

        public static bool TryGetComponentInHierarchy<T>(this GameObject @object, out T comp) where T : Component
        {
            if (!@object.TryGetComponentInChildren<T>(out comp))
            {
                if (@object.transform.parent != null)
                    return @object.transform.parent.TryGetComponentInHierarchy(out comp);
                return false;
            }
            return true;
        }

        public static bool TryGetComponentInHierarchy<T>(this Component @object, out T comp) where T : Component
            => TryGetComponentInHierarchy(@object.gameObject, out comp);

        public static Component EnsureComponent(this GameObject @object, Type type)
        {
            var c = @object.GetComponent(type);
            if (!c) return @object.AddComponent(type);
            return c;
        }

        public static T EnsureComponent<T>(this GameObject @object) where T : Component
        {
            var c = @object.GetComponent<T>();
            if (!c) return @object.AddComponent<T>();
            return c;
        }

        public static Component EnsureComponent(this Component @object, Type type)
            => @object.gameObject.EnsureComponent(type);

        public static T EnsureComponent<T>(this Component @object) where T : Component
            => @object.gameObject.EnsureComponent<T>();

        public static Component Clone(this Component original, GameObject destination)
        {
            var type = original.GetType();
            var copy = destination.EnsureComponent(type);
            var fields = type.GetFields();
            foreach (var field in fields)
                field.SetValue(copy, field.GetValue(original));
            return copy;
        }

        public static T Clone<T>(this T original, GameObject destination) where T : Component
        {
            var type = original.GetType();
            Component copy = destination.EnsureComponent<T>();
            var fields = type.GetFields();
            foreach (var field in fields)
                field.SetValue(copy, field.GetValue(original));
            return copy as T;
        }

        #endregion

        #region Transforms

        public static bool TryFindChild(this Transform t, string name, out Transform child)
            => (child = t.Find(name)) != null;

        public static List<Transform> GetChildren(this Transform t)
        {
            var l = new List<Transform>();
            for (int i = 0; i < t.childCount; i++)
                l.Add(t.GetChild(i));
            return l;
        }

        public static List<Transform> GetHierarchy(this Transform t)
        {
            var children = new List<Transform>();
            foreach (var child in t.GetChildren())
            {
                children.Add(child);
                if (child.childCount > 0)
                    children.AddRange(child.GetHierarchy());
            }
            return children;
        }

        public static List<T> GetChildren<T>(this Transform t) where T : Component
            => t.GetChildren().ConvertAll(q => q.GetComponent<T>()).Where(q => q != null).ToList();

        public static Transform GetChildByName(this Transform t, string name)
            => t.Find(name);

        #endregion

        public static bool IsInLayerMask(this GameObject @object, LayerMask lm)
            => lm == (lm | (1 << @object.layer));

        // todo move to it's own file
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
    }
}