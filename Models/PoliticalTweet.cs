using System;
using Newtonsoft.Json;

namespace PoliticalTweetsImporter.Models
{
    public sealed class PoliticalTweet
    {
        [JsonProperty( PropertyName = "handle" )]
        public string OwnerHandle { get; }

        [JsonProperty( PropertyName = "text" )]
        public string Content { get; }

        [JsonProperty( PropertyName = "date" )]
        public DateTimeOffset Date { get; }

        [JsonProperty( PropertyName = "tweet_id" )]
        public string Id { get; }

        public PoliticalTweet( string handle, string content, DateTimeOffset date, string id )
        {
            OwnerHandle = handle;
            Content = content;
            Date = date;
            Id = id;
        }
    }
}