using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using PoliticalTweetsImporter.Models;
using TweetSharp;

namespace PoliticalTweetsImporter
{
    public static class TweetsImporter
    {
        private const string
                ConsumerKey = "gNc5En2ZKVHqo86hMWr1iRyxv",
                ConsumerSecret = "OkPMufDcJWNBd01Y2SCGg2jhc7uJBWoYEIFtnNgd9bZ7swqxsC",
                AccessToken = "3543553817-oe3YfgSknaQ4FQ77Mutpmyzy3Git9ReTBsz6m3H",
                AccessTokenSecret = "E23J1deJIHGqlpu542TKLHjhPP3j41UTwmCzdM4CvhT8Q";

        public static IReadOnlyCollection<PoliticalTweet> LoadTweets( IEnumerable<string> handles )
        {
            var service = new TwitterService( ConsumerKey, ConsumerSecret );
            service.AuthenticateWith( AccessToken, AccessTokenSecret );
            var results = new List<PoliticalTweet>();

            foreach ( var handle in handles.Distinct() )
            {
                try
                {
                    var response = service.ListTweetsOnUserTimeline( new ListTweetsOnUserTimelineOptions
                    {
                        ScreenName = handle,
                        Count = 200,
                        IncludeRts = false,
                        ExcludeReplies = true
                    } );
                    results.AddRange( response.Select( t => new PoliticalTweet( handle, t.Text, t.CreatedDate, t.IdStr ) ) );

                    Debug.WriteLine( $"{service.Response.RateLimitStatus.RemainingHits} remaining hits." );
                    if ( service.Response.RateLimitStatus.RemainingHits <= 0 )
                    {
                        var wait = service.Response.RateLimitStatus.ResetTime.ToUniversalTime() - DateTime.UtcNow;
                        Debug.WriteLine( $"Rate limit reached. Sleeping for {wait}." );
                        Thread.Sleep( wait );
                    }
                }
                catch
                {
                    Debug.WriteLine( $"Skipping {handle}" );
                }
            }

            return results;
        }

        private sealed class TwitterTweet
        {
            [JsonProperty( PropertyName = "id_str" )]
            public string Id { get; set; }

            [JsonProperty( PropertyName = "text" )]
            public string Text { get; set; }

            [JsonProperty( PropertyName = "created_at" )]
            public string Created { get; set; }
        }
    }
}