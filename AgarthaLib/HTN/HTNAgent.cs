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
        public HTNTask SelectedTask;

        public float ThinkDelay = 1f;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(UpdateAgent());
        }

        protected virtual IEnumerator UpdateAgent()
        {
            yield return new WaitForSeconds(ThinkDelay);

            if (Blackboard == null)
                yield return UpdateAgent();

            foreach (var plan in Blackboard)
            {
                if (plan.Condition != null)
                {
                    plan.Condition.UpdateCondition(this);
                    if (!plan.Condition.ConditionMet)
                        continue;
                }

                SelectedTask = plan.Task;
            }

            if (SelectedTask != null)
            {
                var task = SelectedTask.TaskUpdateEnumerator(this);
                while (task.MoveNext())
                {
                    yield return new WaitForEndOfFrame();
                    if (task.Current == HTNTaskStatus.Continuing) continue;
                    else break;
                }
            }

            SelectedTask = null;
        }
    }

    [Serializable] public class HTNPlan
    {
        public HTNCondition Condition;
        public HTNTask Task;
    }
}
