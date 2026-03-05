using UnityEngine;

namespace AgarthaLib.Attributes
{
    public class ValidateNullAttribute : PropertyAttribute
    {
        public bool Traverse = false;

        public ValidateNullAttribute(bool traverse = false)
            => Traverse = traverse;
    }
}
