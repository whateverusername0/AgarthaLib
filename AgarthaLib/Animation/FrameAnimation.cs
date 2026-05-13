using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    /// <summary>
    ///     An abstract definition of a keyframe based animation.
    /// </summary>
    /// <typeparam name="TFrame"> A concrete frame type. </typeparam>
    public abstract class FrameAnimation<TFrame> : ScriptableObject
        where TFrame : Object
    {
        public List<TFrame> Frames = new();
        public int FPS = 12;
        public bool Loop = true;
    }
}
