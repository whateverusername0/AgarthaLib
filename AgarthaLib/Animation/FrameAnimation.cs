using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Animation
{
    public abstract class FrameAnimation<T> : ScriptableObject where T : Object
    {
        public List<T> Frames = new();
        public int FPS = 12;
        public bool Loop = true;
    }
}
