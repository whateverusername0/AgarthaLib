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

        private Coroutine
            _updateCoroutine = null,
            _updatePlanCoroutine = null;

        protected override void Update()
        {
            base.Update();

            _updateCoroutine ??= StartCoroutine(UpdateCoroutine());
            _updatePlanCoroutine ??= StartCoroutine(UpdatePlanCoroutine());
        }

        protected virtual void OnDisable()
        {
            if (_updateCoroutine != null)
            {
                StopCoroutine(_updateCoroutine);
                _updateCoroutine = null;
            }

            if (_updatePlanCoroutine != null)
            {
                StopCoroutine(_updatePlanCoroutine);
                _updatePlanCoroutine = null;
            }
        }

        protected virtual IEnumerator UpdateCoroutine()
        {
            while (this.isActiveAndEnabled)
            {
                if (CurrentPlan != null)
                {
                    var ie = CurrentPlan.TaskUpdateEnumerator(this);
                    ie.MoveNext();

                    if (ie.IsFinished())
                        CurrentPlan = null;
                }

                yield return null;
            }
        }

        protected virtual IEnumerator UpdatePlanCoroutine()
        {
            while (this.isActiveAndEnabled)
            {
                var bestPlan = Blackboard.GetBestPlan(this);
                if (CurrentPlan != bestPlan)
                {
                    if (CurrentPlan != null)
                        CurrentPlan.ResetPlan();

                    CurrentPlan = bestPlan;
                }

                yield return new WaitForSeconds(PlanChangeDelay);
            }
        }
    }
}
