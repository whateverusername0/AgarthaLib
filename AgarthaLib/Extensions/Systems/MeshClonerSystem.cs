using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Extensions.Systems
{
    public static class MeshClonerSystem
    {
        public static GameObject Clone(GameObject original, Material overrideMaterial = null, bool readHierarchy = false)
        {
            var hierarchy = readHierarchy ? original.transform.GetHierarchy() : original.transform.GetChildren();
            var cloned = new List<GameObject>();
            for (int i = 0; i < hierarchy.Count; i++)
            {
                var child = hierarchy[i].gameObject;
                var g = new GameObject(hierarchy[i].name);
                g.transform.SetLocalPositionAndRotation(child.transform.localPosition, child.transform.localRotation);
                g.transform.localScale = child.transform.localScale;

                if (child.TryGetComponent<MeshRenderer>(out var renderer)
                && child.TryGetComponent<MeshFilter>(out var filter)
                && filter.mesh != null)
                {
                    filter.Clone(g);
                    var mr = renderer.Clone(g);
                    mr.material = overrideMaterial != null ? overrideMaterial : renderer.material;
                }

                cloned.Add(child);
            }
            return cloned[0]; // always exists.
        }
    }
}
