using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.HTN
{
    public class HTNConditionComposite : HTNCondition
    {
        public List<HTNCondition> Conditions;
        public bool ConditionsMet = false;

        protected override bool CheckCondition(HTNAgent agent)
        {
            foreach (var cond in Conditions) cond.UpdateCondition(agent);
            ConditionsMet = Conditions.All(q => q.ConditionMet);
            return ConditionsMet;
        }
    }
}
