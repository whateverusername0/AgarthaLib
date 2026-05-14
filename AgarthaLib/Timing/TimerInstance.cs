using System;

namespace AgarthaLib.Timing
{
    [Serializable] public class TimerInstance
    {
        public float Delay;
        public float Timer;
        public Action Action;

        public TimerInstance(float delay, Action action)
        {
            Delay = delay;
            Timer = delay;
            Action = action;
        }
    }
}
