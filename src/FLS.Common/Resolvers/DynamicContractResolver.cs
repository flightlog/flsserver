using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace FLS.Common.Resolvers
{
    /// <summary>
    /// http://www.tecsupra.com/serializing-only-some-properties-of-an-object-to-json-using-newtonsoft-json-net/
    /// </summary>
    public class DynamicContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        private readonly IList<string> _propertiesToSerialize = null;

        public DynamicContractResolver(IList<string> propertiesToSerialize)
        {
            _propertiesToSerialize = propertiesToSerialize;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => _propertiesToSerialize.Contains(p.PropertyName)).ToList();
        }
    }
}
