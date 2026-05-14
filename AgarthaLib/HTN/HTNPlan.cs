using AgarthaLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.HTN
{
    [Serializable] public class HTNPlan : HTNTask
    {
        public List<SerializedHTNTaskData> Tasks = new();
        [EditorReadOnly] public HTNTaskStatus Status = HTNTaskStatus.Waiting;

        [Tooltip("If true, tasks will update simultaneously. If false, tasks will update one by one.")]
        public bool UpdateConcurrently = true;

        [Tooltip("If true, will automatically reset the plan once all tasks are done.")]
        public bool AutoResetPlan = true;

        public override IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent)
        {
            if (Tasks == null || Tasks.Count == 0)
                yield return HTNTaskStatus.Completed; // assume it's done

            var ie = UpdateConcurrently
                ? ConcurrentTaskUpdateEnumerator(agent)
                : OrderedTaskUpdateEnumerator(agent);

            ie.MoveNext();

            Status = ie.Current;

            if (!ie.IsRunning() && AutoResetPlan)
                ResetPlan();

            yield return ie.Current;
        }

        private IEnumerator<HTNTaskStatus> ConcurrentTaskUpdateEnumerator(HTNAgent agent)
        {
            foreach (var task in Tasks)
            {
                var ie = task.Component.TaskUpdateEnumerator(agent);

                // assume it's completed and does not require iteration
                if (!task.IsRunning())
                    continue;

                ie.MoveNext();

                task.Status = ie.Current;
            }

            if (!Tasks.Any(q => q.IsRunning()))
            {
                var failed = Tasks.All(q => q.Status == HTNTaskStatus.Failed);
                yield return failed ? HTNTaskStatus.Failed : HTNTaskStatus.Completed;
            }

            yield return HTNTaskStatus.Continuing;
        }

        private IEnumerator<HTNTaskStatus> OrderedTaskUpdateEnumerator(HTNAgent agent)
        {
            foreach (var task in Tasks)
            {
                if (task.IsFinished())
                    continue;

                var ie = task.Component.TaskUpdateEnumerator(agent);
                ie.MoveNext();

                task.Status = ie.Current;

                if (ie.Current == HTNTaskStatus.Failed)
                    yield return HTNTaskStatus.Failed;

                break;
            }

            if (Tasks.Any(q => q.IsRunning()))
                yield return HTNTaskStatus.Continuing;
            else yield return HTNTaskStatus.Completed;
        }

        public void ResetPlan()
        {
            Status = HTNTaskStatus.Waiting;
            foreach (var task in Tasks)
                task.Status = HTNTaskStatus.Waiting;
        }
    }
}
