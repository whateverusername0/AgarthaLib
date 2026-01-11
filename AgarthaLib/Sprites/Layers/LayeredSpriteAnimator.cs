using AgarthaLib.MonoBehavior;
using AgarthaLib.Sprites.Layers.Rendering;

namespace AgarthaLib.Sprites.Layers
{
    /// <summary>
    ///     This is a copy of <see cref="Animation.SpriteAnimator.SpriteAnimatorBase"/> but with layer support.
    ///     This is because there's a lot to layer maps than there is to normal sprites.
    /// </summary>
    public class LayeredSpriteAnimator : AgarthanBehaviour
    {
        public LayeredSpriteRendererBase Renderer;
    }
}
