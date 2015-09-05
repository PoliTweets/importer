using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PoliticalTweetsImporter
{
    public sealed class DjangoContainer<T>
    {
        [JsonProperty( PropertyName = "pk", NullValueHandling = NullValueHandling.Include )]
        public string PrimaryKey { get; set; } = null;

        [JsonProperty( PropertyName = "model" )]
        public string ModelName { get; set; }

        [JsonProperty( PropertyName = "fields" )]
        public T Value { get; set; }


        public DjangoContainer( string modelName, T value )
        {
            ModelName = "politweets." + modelName;
            Value = value;
        }
    }

    public static class DjangoContainer
    {
        public static string Wrap<T>( string modelName, IEnumerable<T> values )
        {
            return JsonConvert.SerializeObject( values.Select( v => new DjangoContainer<T>( modelName, v ) ), Formatting.Indented );
        }

        public static IEnumerable<T> Unwrap<T>( string json )
        {
            return JsonConvert.DeserializeObject<IEnumerable<DjangoContainer<T>>>( json ).Select( c => c.Value ).ToArray();
        }
    }
}