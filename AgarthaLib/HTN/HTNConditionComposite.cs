using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.HTN
{
    public class HTNConditionComposite : HTNCondition
    {
        public List<HTNCondition> Conditions;
        public bool ConditionsMet = false;

        protected override bool CheckCondition()
        {
            foreach (var cond in Conditions) cond.UpdateCondition();
            ConditionsMet = Conditions.All(q => q.ConditionMet);
            return ConditionsMet;
        }
    }
}
