using UnityEngine;

namespace Agartha.HTN
{
    public abstract class HTNCondition : MonoBehaviour
    {
        public bool ConditionMet = false;

        public void UpdateCondition()
            => ConditionMet = CheckCondition();

        protected abstract bool CheckCondition();
    }
}
