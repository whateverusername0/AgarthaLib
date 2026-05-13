using AgarthaLib.MonoBehavior;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.HTN
{
    public class HTNBlackboard : AgarthanBehaviour
    {
        [Tooltip("Priority is sorted by the list's order. Index 0 has more priority over index 1.")]
        public List<HTNPlan> Plans;
    }
}
