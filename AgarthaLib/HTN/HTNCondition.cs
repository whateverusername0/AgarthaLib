using UnityEngine;

namespace AgarthaLib.HTN
{
    public abstract class HTNCondition : MonoBehaviour
    {
        public bool ConditionMet = false;

        public void UpdateCondition()
            => ConditionMet = CheckCondition();

        protected abstract bool CheckCondition();
    }
}
