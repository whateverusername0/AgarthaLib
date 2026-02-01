using AgarthaLib.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageSpriteAnimator : SpriteAnimatorBase
    {
        [ValidateNull] public RawImage Image;

        protected override void SetFrame(Sprite frame)
            => Image.texture = frame.texture;
    }
}