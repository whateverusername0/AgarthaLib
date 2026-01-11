using UnityEngine;
using UnityEngine.UI;

namespace AgarthaLib.Sprites.Animation.SpriteAnimator
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageSpriteAnimator : SpriteAnimatorBase
    {
        public RawImage Image;

        private void Start()
        {
            Image = Image == null ? GetComponent<RawImage>() : Image;
        }

        protected override void SetFrame(Sprite frame)
            => Image.texture = frame.texture;
    }
}