#if USING_NEWTONSOFT_JSON
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace AgarthaLib.Data.JSON
{
    public class FieldsOnlyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (member.MemberType == MemberTypes.Property)
            {
                prop.ShouldSerialize = _ => false;
                prop.ShouldDeserialize = _ => false;
                prop.Ignored = true;
            }

            return prop;
        }
    }
}
#endif