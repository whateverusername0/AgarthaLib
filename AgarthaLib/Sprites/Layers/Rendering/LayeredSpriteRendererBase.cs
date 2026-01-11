using AgarthaLib.Extensions;
using AgarthaLib.MonoBehavior;
using System.Linq;

namespace AgarthaLib.Sprites.Layers.Rendering
{
    public abstract class LayeredSpriteRendererBase : AgarthanBehaviour
    {
        public LayeredSprite Sprite;
        protected SpriteLayerMap _lastMap;

        protected void Update()
        {
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
            var invalidLayers = layers.Where(q => !map.Map.Any(w => w.Name == q.name)).ToList();

            // the layer exists on the map but not in the renderer
            var requiredLayers = map.Map.Where(q => !layers.Any(w => w.name == q.Name)).ToList();

            foreach (var l in invalidLayers)
                Destroy(l.gameObject);

            foreach (var l in requiredLayers)
                CreateLayer(l);

            // the layer exists on both but is different
            var layersToUpdate = map.Map.Where(q => lastMap.TryGetLayer(q.Name, out var l) && q != l).ToList();
            foreach (var l in layersToUpdate)
                UpdateLayer(l);

            SortLayers();
        }

        protected abstract void CreateLayer(SpriteLayer layer);

        protected abstract void UpdateLayer(SpriteLayer layer);

        protected abstract void SortLayers();
    }
}
