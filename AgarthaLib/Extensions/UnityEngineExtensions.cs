using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace AgarthaLib.Extensions
{
    public static class UnityEngineExtensions
    {
        #region Objects

        public static void SafeDestroy(this UnityEngine.Object _, UnityEngine.Object obj)
        {
            if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
            else UnityEngine.Object.DestroyImmediate(obj);
        }

        public static bool TryFindObjectOfType<T>(this UnityEngine.Object @object, out T @out)
            where T : UnityEngine.Object
        {
            @out = UnityEngine.Object.FindObjectOfType<T>();
            return @out != null;
        }

        public static T TryInstantiate<T>(this UnityEngine.Object @object, GameObject original,
            Vector3 position, Quaternion rotation) where T : UnityEngine.Component
        {
            var inst = UnityEngine.Object.Instantiate(original, position, rotation);
            if (inst.TryGetComponentInChildren<T>(out var c))
                return c;

            inst.SafeDestroy(inst);
            return null;
        }

        #endregion

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

        public static Component Clone(this Component original, Component destination)
        {
            var type = original.GetType();
            var fields = type.GetFields();
            foreach (var field in fields)
                field.SetValue(destination, field.GetValue(original));
            return destination;
        }

        public static T Clone<T>(this T original, T destination) where T : Component
        {
            var type = original.GetType();
            var fields = type.GetFields();
            foreach (var field in fields)
                field.SetValue(destination, field.GetValue(original));
            return destination as T;
        }

        public static Component Clone(this Component original, GameObject destination)
        {
            var type = original.GetType();
            var comp = destination.EnsureComponent(type);
            return original.Clone(comp);
        }

        public static T Clone<T>(this T original, GameObject destination) where T : Component
        {
            var comp = destination.EnsureComponent<T>();
            return original.Clone(comp);
        }

        #endregion

        #region Transforms

        public static bool TryFindChild(this Transform t, string name, out Transform child)
            => (child = t.Find(name)) != null;

        public static Transform EnsureChild(this Transform t, string name)
        {
            if (t.TryFindChild(name, out var tr))
                return tr;

            var go = new GameObject(name);
            go.transform.SetParent(t.transform, false);
            return go.transform;
        }

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

        /// <inheritdoc cref="Transform.Find(string)"/>
        public static Transform GetChildByName(this Transform t, string name)
            => t.Find(name);

        public static Vector3 LookDirection(this Transform t, Transform target)
            => target.position - t.position;

        #endregion

        #region Rendering

        public static bool IsInLayerMask(this GameObject @object, LayerMask lm)
            => lm == (lm | (1 << @object.layer));

        // TODO: move to it's own file
        public static bool IsVisibleFrom(this Bounds bounds, Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
            => renderer.bounds.IsVisibleFrom(camera);

        /// <summary>
        ///     Gets a pipeline asset.
        /// </summary>
        /// <param name="ass">The used scriptable pipeline.</param>
        /// <returns>If a scriptable pipeline is being used. If not, it's a Built-in renderer.</returns>
        public static bool TryGetUsedPipeline(out RenderPipelineAsset ass)
        {
            var graphicsPipeline = GraphicsSettings.defaultRenderPipeline;
            var qualityPipeline = QualitySettings.renderPipeline;

            // use qualitypipeline as fallback
            ass = graphicsPipeline ? graphicsPipeline : qualityPipeline;
            return ass != null;
        }

        #endregion

        #region Uncategorized

        public static bool IsInLayerMask(this LayerMask lm, int layer)
            => lm == (lm | (1 << layer));

        #endregion
    }
}