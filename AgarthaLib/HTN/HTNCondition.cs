using AgarthaLib.MonoBehavior;
using System;

namespace AgarthaLib.HTN
{
    public interface IHTNCondition
    {
        public bool CheckCondition(HTNAgent agent);
    }

    public abstract class HTNCondition : AgarthanBehaviour, IHTNCondition
    {
        // plans get their conditions handled in HTNAgent
        // meanwhile HTNTasks will have to write their own condition handlers.
        public abstract bool CheckCondition(HTNAgent agent);
    }
}
