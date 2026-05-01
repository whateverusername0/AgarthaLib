using AgarthaLib.Attributes;
using AgarthaLib.MonoBehavior;
using System.Collections;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public class HTNAgent : AgarthanBehaviour
    {
        public HTNBlackboard Blackboard;
        [EditorReadOnly] public HTNPlan CurrentPlan;

        public float PlanChangeDelay = 1f;

        private Coroutine _coroutine = null;

        protected override void Update()
        {
            base.Update();

            if (_coroutine == null)
                StartCoroutine(UpdateCoroutine());
        }

        protected virtual void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        protected virtual IEnumerator UpdateCoroutine()
        {
            while (this.isActiveAndEnabled)
            {
                yield return new WaitForSeconds(PlanChangeDelay);

                var plans = Blackboard.Plans;
                foreach (var plan in plans)
                {
                    if (!CheckCondition(plan.Condition))
                        continue;

                    CurrentPlan = plan;
                    break;
                }

                if (CurrentPlan == null)
                    continue;

                var ie = CurrentPlan.TaskUpdateEnumerator(this);
                while (ie.MoveNext())
                    yield return ie.Current;

                CurrentPlan = null;
            }
        }

        private bool CheckCondition(SerializedHTNConditionData condition)
            => condition == null || (condition.ConditionMet || condition.Component.CheckCondition(this));
    }
}
