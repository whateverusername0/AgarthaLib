using AgarthaLib.MonoBehavior;
using System.Collections.Generic;

namespace AgarthaLib.Tags
{
    public class TagsContainer : AgarthanBehaviour, ITagsContainer
    {
        public List<string> Tags = new();
        public List<string> GetTags() => Tags;
    }
}
