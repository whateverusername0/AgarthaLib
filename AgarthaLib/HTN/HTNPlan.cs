using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.HTN
{
    [Serializable] public class HTNPlan : HTNTask
    {
        public List<SerializedHTNTaskData> Tasks = new();
        public HTNTaskStatus Status = HTNTaskStatus.Waiting;

        [Tooltip("If true, tasks will update simultaneously. If false, tasks will update one by one.")]
        public bool UpdateConcurrently = true;

        [Tooltip("If true, will automatically reset the plan once all tasks are done.")]
        public bool AutoResetPlan = false;

        public override IEnumerator<HTNTaskStatus> TaskUpdateEnumerator(HTNAgent agent)
        {
            if (Tasks == null || Tasks.Count == 0)
                yield return HTNTaskStatus.Completed; // assume it's done

            var ie = UpdateConcurrently
                ? ConcurrentTaskUpdateEnumerator(agent)
                : OrderedTaskUpdateEnumerator(agent);

            while (ie.MoveNext())
            {
                yield return ie.Current;
                Status = ie.Current;

                if (!ie.IsRunning())
                    break;
            }

            // assume the plan is complete
            if (AutoResetPlan) ResetPlan(agent);
            yield return Status;
        }

        private IEnumerator<HTNTaskStatus> ConcurrentTaskUpdateEnumerator(HTNAgent agent)
        {
            do
            {
                foreach (var task in Tasks)
                {
                    var ie = task.Component.TaskUpdateEnumerator(agent);

                    // assume it's completed and does not require iteration
                    if (!ie.IsRunning())
                        continue;

                    ie.MoveNext();
                    task.Status = ie.Current;

                    yield return HTNTaskStatus.Continuing;
                }
            } while (Tasks.Any(q => q.IsRunning()));

            // if all tasks failed assume the plan is too
            var failed = Tasks.All(q => q.Status == HTNTaskStatus.Failed);
            yield return failed ? HTNTaskStatus.Failed : HTNTaskStatus.Completed;
        }

        private IEnumerator<HTNTaskStatus> OrderedTaskUpdateEnumerator(HTNAgent agent)
        {
            foreach (var task in Tasks)
            {
                do
                {
                    var ie = task.Component.TaskUpdateEnumerator(agent);
                    ie.MoveNext();

                    yield return ie.Current;
                    task.Status = ie.Current;

                    // since it's ordered, if one thing fails the rest falls like a domino.
                    if (ie.Current == HTNTaskStatus.Failed)
                    {
                        yield return HTNTaskStatus.Failed;
                        yield break;
                    }
                } while (task.Status == HTNTaskStatus.Continuing);
            }

            yield return HTNTaskStatus.Completed;
        }

        public void ResetPlan(HTNAgent agent)
        {
            Status = HTNTaskStatus.Waiting;
            foreach (var task in Tasks)
            {
                task.Component.TaskUpdateEnumerator(agent).Reset();
                task.Status = HTNTaskStatus.Waiting;
            }
        }
    }
}
