namespace AgarthaLib.Animation.Sprite.Image
{
    public class ImageAnimator : FrameAnimator<SpriteAnimation, UnityEngine.Sprite>
    {
        public UnityEngine.UI.Image Renderer;

        protected override void SetFrame(UnityEngine.Sprite frame)
            => Renderer.sprite = frame;
    }
}