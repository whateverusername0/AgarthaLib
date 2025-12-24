using System.Collections.Generic;
using UnityEngine;

namespace Agartha.SpriteAnimation
{
    /// <summary>
    ///     Second base class. Is used as a data structure in child classes.
    /// </summary>
    [CreateAssetMenu(menuName = "Agartha / Sprite animations / Sprite animation")]
    public class SpriteAnimation : SpriteAnimationBase
    {
        public int FPS = 12;
        public List<Sprite> Frames;
        public bool Loop = true;

        public SpriteAnimation(int fps, List<Sprite> frames, bool loop)
        {
            FPS = fps;
            Frames = frames;
            Loop = loop;
        }

        public SpriteAnimation Reverse()
        {
            var newFrames = new List<Sprite>(Frames);
            newFrames.Reverse();

            var @new = CreateInstance<SpriteAnimation>();
            @new.FPS = FPS;
            @new.Frames = newFrames;
            @new.Loop = Loop;

            return @new;
        }
    }
}