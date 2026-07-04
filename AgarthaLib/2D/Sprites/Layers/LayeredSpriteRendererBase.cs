using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib._2D.Sprites.Layers
{
    [ExecuteInEditMode]
    public abstract class LayeredSpriteRendererBase<T> : AgarthanBehaviour where T : Component
    {
        public LayeredSprite Sprite;
        protected List<SpriteLayer> _lastMap;
        public int OrderInLayer = 0;

        protected override void Update()
        {
            base.Update();

            if (Sprite.LayerMap != _lastMap)
                UpdateLayerMap();

            _lastMap = Sprite.LayerMap;
        }

        public void UpdateLayerMap()
        {
            if (Sprite == null) return;
            var map = Sprite.LayerMap;
            var lastMap = _lastMap;
            var layers = transform.GetChildren();

            // the layer exists in the renderer but not on the map
            var invalidLayers = layers.Where(q => !map.Any(w => w.Name == q.name)).ToList();

            // the layer exists on the map but not in the renderer
            var requiredLayers = map.Where(q => !layers.Any(w => w.name == q.Name)).ToList();

            foreach (var l in invalidLayers)
                Destroy(l.gameObject);

            foreach (var l in requiredLayers)
                CreateLayer(l);

            // the layer exists on both but is different
            var layersToUpdate = map.Where(q => lastMap.FirstOrDefault(w => w.Name == q.Name) != q);
            foreach (var l in layersToUpdate)
                UpdateLayer(l);
        }

        protected virtual Transform CreateLayer(SpriteLayer layer)
        {
            var layerTransform = new GameObject(layer.Name).transform;
            layerTransform.SetParent(transform);
            UpdateLayer(layerTransform, layer);
            return layerTransform;
        }

        protected virtual void UpdateLayer(SpriteLayer layer)
        {
            if (!TryGetLayer(layer, out var lt))
            {
                Debug.LogWarning($"SpriteLayer {layer.Name} does not exist in {transform.name}");
                CreateLayer(layer);
            }

            UpdateLayer(lt, layer);
        }

        protected virtual void UpdateLayer(Transform lt, SpriteLayer layer)
        {
            var renderer = lt.EnsureComponent<T>();

            SetSprite(renderer, layer.Sprite);

            if (layer.Material != null)
                SetMaterial(renderer, layer.Material);

            SetOrderInLayer(renderer, GetOrderInLayer(renderer) + layer.SortingLayer);
        }

        public virtual bool TryGetLayer(SpriteLayer layer, out Transform layerTransform)
        {
            layerTransform = transform.GetChildByName(layer.Name);
            return layerTransform != null;
        }

        protected abstract void SetSprite(T renderer, Sprite sprite);

        protected abstract void SetMaterial(T renderer, Material mat);

        protected abstract void SetOrderInLayer(T renderer, int order);

        protected abstract int GetOrderInLayer(T renderer);
    }
}
