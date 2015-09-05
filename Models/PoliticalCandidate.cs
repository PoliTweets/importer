using Newtonsoft.Json;

namespace PoliticalTweetsImporter.Models
{
    public sealed class PoliticalCandidate
    {
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; }

        [JsonProperty( PropertyName = "full_party_name" )]
        public string PartyName { get; }

        [JsonProperty( PropertyName = "canton" )]
        public string CantonName { get; }

        [JsonProperty( PropertyName = "handle" )]
        public string TwitterHandle { get; }

        public PoliticalCandidate( string name, string partyName, string cantonName, string handle )
        {
            Name = name;
            PartyName = partyName;
            CantonName = cantonName;
            TwitterHandle = handle;
        }
    }
}