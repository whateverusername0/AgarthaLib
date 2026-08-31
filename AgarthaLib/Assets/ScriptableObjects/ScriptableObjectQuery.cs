using AgarthaLib.Attributes;
using AgarthaLib.Data;
using AgarthaLib.Data.Serialization.SerializedTypes;
using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Assets.ScriptableObjects
{
    public class ScriptableObjectQuery : MonoSingleton<ScriptableObjectQuery>, IDoNotDestroyOnLoad
    {
        [SerializeField, EditorReadOnly]
        private SerializedDictionary<Type, List<ScriptableObject>> Query = new();

        private void OnDestroy()
        {
            while (Query.Count > 0)
            {
                var first = Query.First();
                ClearQuery(first.Value);
                Query.Remove(first.Key);
            }
        }

        public List<T> GetObjectsOfType<T>() where T : ScriptableObject
        {
            var query = new List<T>();
            foreach (var l in Query)
            {
                var canConvert = typeof(T).IsAssignableFrom(l.Key.GetType());
                if (!canConvert) continue;

                foreach (var v in l.Value)
                {
                    if (v == null) continue;
                    query.Add(v as T);
                }
            }

            return query;
        }

        public T Instantiate<T>() where T : ScriptableObject
        {
            var so = ScriptableObject.CreateInstance<T>();
            so.hideFlags = HideFlags.DontUnloadUnusedAsset;

            if (!Query.ContainsKey(typeof(T)))
                Query.Add(typeof(T), new());
            Query[typeof(T)].Add(so);

            return so;
        }

        private void ClearQuery(List<ScriptableObject> l)
        {
            if (l == null || l.Count == 0) return;
            foreach (var v in l) this.SafeDestroy(v);
        }
    }
}
