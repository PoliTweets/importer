using System;
using System.IO;
using System.Linq;

namespace PoliticalTweetsImporter
{
    public sealed class Program
    {
        public static void Main( string[] args )
        {
            var candidates = CandidatesExcelImporter.LoadCandidates( @"X:\candidats.xlsx" );
            var tweets = TweetsImporter.LoadTweets( candidates.Select( c => c.TwitterHandle ) );

            File.WriteAllText( "candidates.json", DjangoContainer.Wrap( "candidate", candidates ) );
            File.WriteAllText( "tweets.json", DjangoContainer.Wrap( "tweet", tweets ) );

            Console.WriteLine( "Done." );
            Console.Read();
        }
    }
}