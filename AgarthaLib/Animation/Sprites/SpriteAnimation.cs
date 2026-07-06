using AgarthaLib.Attributes;
using UnityEngine;

namespace AgarthaLib.Animation.Sprites
{
    [CreateAssetMenu(menuName = "AgarthaLib / Animation / Sprite / Sprite Animation")]
    public sealed class SpriteAnimation : FrameAnimation<Sprite>
    {
        [ScriptableObjectIcon] public Sprite Icon
            => Frames.Count > 0 ? Frames[0] : null;
    }
}
