using System;

namespace AgarthaLib.Goodies.Timing
{
    [Serializable] public class TimerInstance
    {
        public float Delay;
        public float Timer;
        public Action Action;
        public bool DoOnce = true;

        public TimerInstance(float delay, Action action)
        {
            Delay = delay;
            Timer = delay;
            Action = action;
        }
    }
}
