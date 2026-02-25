using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Tags
{
    [CreateAssetMenu(menuName = "AgarthaLib / Tags / Tags definition")]
    public class TagsDefinition : ScriptableObject, ITagsContainer
    {
        public List<string> Tags = new();
        public List<string> GetTags() => Tags;
    }
}
