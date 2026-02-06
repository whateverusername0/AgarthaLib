using AgarthaLib.MonoBehavior;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public class HTNAgent : AgarthanBehaviour
    {
        /// <summary>
        ///     The higher the number, the higher it's priority is.
        ///     Means 0 is the lowest.
        /// </summary>
        public List<HTNPlan> Blackboard;
        public HTNTaskData SelectedTask;

        public float ThinkDelay = 1f;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(UpdateAgent());
        }

        protected virtual IEnumerator UpdateAgent()
        {
            while (true)
            {
                yield return new WaitForSeconds(ThinkDelay);

                if (Blackboard == null || Blackboard.Count == 0)
                    continue;

                foreach (var plan in Blackboard)
                {
                    if (plan.Condition != null)
                    {
                        plan.Condition.UpdateCondition(this);
                        if (!plan.Condition.ConditionMet)
                            continue;
                    }
                    SelectedTask = new(plan.Task);
                }

                if (SelectedTask != null)
                {
                    var task = SelectedTask.Task.TaskUpdateEnumerator(this);
                    while (task.MoveNext())
                    {
                        yield return null;
                        SelectedTask.Status = task.Current;

                        if (task.Current == HTNTaskStatus.Continuing) continue;
                        else break;
                    }
                }

                SelectedTask = null;
            }
        }
    }

    [Serializable] public class HTNPlan
    {
        public HTNCondition Condition;
        public HTNTask Task;
    }

    [Serializable] public class HTNTaskData
    {
        public HTNTask Task;
        public HTNTaskStatus? Status;

        public HTNTaskData(HTNTask task)
        {
            Task = task;
        }
    }
}
