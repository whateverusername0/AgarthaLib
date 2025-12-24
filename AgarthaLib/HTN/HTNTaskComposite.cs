using System.Collections.Generic;
using UnityEngine;

namespace Agartha.HTN
{
    public class HTNTaskComposite : HTNTask
    {
        public List<HTNTask> Tasks;
        public HTNCondition Condition;
        public float ConditionUpdateCheck = 1f;
        private float _conditionTimer = 0f;
        public bool CanExecute = true;

        /// <summary>
        ///     Made into a separate function from Update because HTNAgent utilizes it.
        ///     And without an HTNAgent this would be useless!
        /// </summary>
        public override void UpdateTask()
        {
            if (Condition != null)
            {
                _conditionTimer += Time.deltaTime;
                if (_conditionTimer >= ConditionUpdateCheck)
                {
                    _conditionTimer = 0f;
                    Condition.UpdateCondition();
                }

                CanExecute = Condition.ConditionMet;
                if (!CanExecute) return;
            }

            foreach (var task in Tasks)
                task.UpdateTask();
        }
    }
}
