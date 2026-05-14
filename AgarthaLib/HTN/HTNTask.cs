using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System;
using System.Collections.Generic;

namespace AgarthaLib.HTN
{
    public interface IHTNTask
    {
        public IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent);
    }

    public abstract class HTNTask : AgarthanBehaviour, IHTNTask
    {
        public HTNCondition Condition;

        /// <summary>
        ///     A method which will supposedly be executed each frame.
        /// </summary>
        /// <remarks>
        ///     do/while loops are highly discouraged,
        ///     since HTNAgents are supposed to handle that for the tasks.
        /// </remarks>
        public abstract IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent);
    }

    [Serializable] public class SerializedHTNTaskData
    {
        public HTNTask Component;
        [EditorReadOnly] public HTNTaskStatus Status = HTNTaskStatus.Waiting;

        public SerializedHTNTaskData(HTNTask task)
            => Component = task;
    }

    [Serializable] public enum HTNTaskStatus
    {
        Waiting,
        Continuing,
        Completed,
        Failed,
    }

    public static class HTNTaskStatusExtensions
    {
        public static bool IsRunning(this IEnumerator<HTNTaskStatus> ie)
            => ie.Current == HTNTaskStatus.Continuing || ie.Current == HTNTaskStatus.Waiting;

        public static bool IsFinished(this IEnumerator<HTNTaskStatus> ie)
            => ie.Current == HTNTaskStatus.Completed || ie.Current == HTNTaskStatus.Failed;

        public static bool IsRunning(this SerializedHTNTaskData ie)
            => ie.Status == HTNTaskStatus.Continuing || ie.Status == HTNTaskStatus.Waiting;

        public static bool IsFinished(this SerializedHTNTaskData ie)
            => ie.Status == HTNTaskStatus.Completed || ie.Status == HTNTaskStatus.Failed;
    }
}
