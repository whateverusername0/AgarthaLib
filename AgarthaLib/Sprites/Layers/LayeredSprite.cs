using AgarthaLib.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.Sprites.Layers
{
    [CreateAssetMenu(menuName = "AgarthaLib / Sprites / Layered sprite")]
    public class LayeredSprite : ScriptableObject
    {
        public List<SpriteLayer> LayerMap = new();

        [ScriptableObjectIcon, SerializeField]
        private Sprite _icon => LayerMap != null && LayerMap.First() != null ? LayerMap.First().Sprite : null;
    }
}
