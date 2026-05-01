using System.Collections.Generic;
using System.Linq;

namespace AgarthaLib.HTN
{
    public class HTNConditionComposite : HTNCondition
    {
        public List<HTNCondition> Conditions;

        public override bool CheckCondition(HTNAgent agent)
            => Conditions.All(q => q.CheckCondition(agent));
    }
}
