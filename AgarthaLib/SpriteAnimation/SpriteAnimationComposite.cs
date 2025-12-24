using System.Collections.Generic;
using UnityEngine;

namespace Agartha.SpriteAnimation
{
    /// <summary>
    ///     Third base class. Unwraps into a list of sprite animations.
    /// </summary>
    [CreateAssetMenu(menuName = "Agartha / Sprite animations / Sprite animation composite")]
    public class SpriteAnimationComposite : SpriteAnimationBase
    {
        public List<SpriteAnimation> Animations;

        public static implicit operator List<SpriteAnimation>(SpriteAnimationComposite a)
            => a.Animations;
    }
}