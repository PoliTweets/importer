using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PoliticalTweetsImporter
{
    public sealed class DjangoContainer
    {
        [JsonProperty( PropertyName = "pk", NullValueHandling = NullValueHandling.Include )]
        public string PrimaryKey { get; } = null;

        [JsonProperty( PropertyName = "model" )]
        public string ModelName { get; }

        [JsonProperty( PropertyName = "fields" )]
        public object Value { get; }


        public DjangoContainer( string modelName, object value )
        {
            ModelName = "politweets." + modelName;
            Value = value;
        }


        public static string Wrap( string modelName, IEnumerable<object> values )
        {
            return JsonConvert.SerializeObject( values.Select( v => new DjangoContainer( modelName, v ) ), Formatting.Indented );
        }
    }
}