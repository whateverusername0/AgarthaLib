using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;

namespace AgarthaLib.HTN
{
    /// <summary>
    ///     Serialized task that works via <see cref="AgarthanBehaviour"/>.
    /// </summary>
    public abstract class HTNTask : AgarthanBehaviour, IHTNTask
    {
        public abstract IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent);
    }

    /// <summary>
    ///     Non serialized task that can have it's own constructor.
    /// </summary>
    public abstract class HTNTaskNonSerialized : IHTNTask
    {
        public abstract IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent);
    }

    public interface IHTNTask
    {
        public IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent);
    }

    [Serializable] public enum HTNTaskStatus
    {
        Completed,
        Continuing,
        Failed,
        Unknown
    }
}
