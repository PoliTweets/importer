using System;
using Newtonsoft.Json;

namespace PoliticalTweetsImporter.Models
{
    public sealed class PoliticalTweet
    {
        [JsonProperty( PropertyName = "handle" )]
        public string OwnerHandle { get; set; }

        [JsonProperty( PropertyName = "text" )]
        public string Content { get; set; }

        [JsonProperty( PropertyName = "date" )]
        public DateTimeOffset Date { get; set; }

        [JsonProperty( PropertyName = "tweet_id" )]
        public string Id { get; set; }


        public PoliticalTweet( string handle, string content, DateTimeOffset date, string id )
        {
            OwnerHandle = handle;
            Content = content;
            Date = date;
            Id = id;
        }
    }
}