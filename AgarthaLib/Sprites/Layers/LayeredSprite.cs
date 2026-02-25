using AgarthaLib.Attributes;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Sprites.Layers
{
    [CreateAssetMenu(menuName = "AgarthaLib / Sprites / Layered sprite")]
    public class LayeredSprite : ScriptableObject
    {
        public SpriteLayerMap LayerMap = new();

        [ScriptableObjectIcon, EditorReadOnly, SerializeField]
        private Sprite _icon => LayerMap.Map.Count > 0 ? LayerMap.Map.First() : null;
    }
}
