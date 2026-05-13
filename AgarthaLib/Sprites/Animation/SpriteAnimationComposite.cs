using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Sprites.Animation
{
    /// <summary>
    ///     Third base class. Unwraps into a list of sprite animations.
    /// </summary>
    [CreateAssetMenu(menuName = "AgarthaLib / Sprite animations / Sprite animation composite")]
    [Obsolete("Please use the Animation.Sprite namespace instead")]
    public class SpriteAnimationComposite : SpriteAnimationBase
    {
        public List<SpriteAnimation> Animations;

        public static implicit operator List<SpriteAnimation>(SpriteAnimationComposite a)
            => a.Animations;
    }
}