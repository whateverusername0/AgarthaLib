using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public class HTNBlackboard : AgarthanBehaviour
    {
        [Tooltip("Priority is sorted by the list's order. Index 0 has more priority over index 1.")]
        public List<HTNPlan> Plans;

        public HTNPlan GetBestPlan(HTNAgent agent)
            => Plans.FirstOrDefault(q => CheckCondition(q.Condition, agent));

        public bool CheckCondition(HTNCondition condition, HTNAgent agent)
            => condition == null || condition.CheckCondition(agent);
    }
}
