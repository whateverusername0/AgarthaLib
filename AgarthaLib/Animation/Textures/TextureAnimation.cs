using AgarthaLib.Attributes;
using UnityEngine;

namespace AgarthaLib.Animation.Textures
{
    [CreateAssetMenu(menuName = "AgarthaLib / Animation / Textures / Texture Animation")]
    public class TextureAnimation : FrameAnimation<Texture>
    {
        [ScriptableObjectIcon] public Texture Icon
            => Frames.Count > 0 ? Frames[0] : null;
    }
}
