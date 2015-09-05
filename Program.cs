using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PoliticalTweetsImporter.Models;

namespace PoliticalTweetsImporter
{
    public sealed class Program
    {
        public static void Main( string[] args )
        {
            Process();

            Console.WriteLine( "Done." );
            Console.Read();
        }

        private static void DownloadTweets()
        {
            var candidates = CandidatesExcelImporter.LoadCandidates( @"X:\candidats.xlsx" );
            var tweets = TweetsImporter.LoadTweets( candidates.Select( c => c.TwitterHandle ) );

            File.WriteAllText( "candidates.json", DjangoContainer.Wrap( "candidate", candidates ) );
            File.WriteAllText( "tweets.json", DjangoContainer.Wrap( "tweet", tweets ) );
        }

        private static void Process()
        {
            string candidatesJson = File.ReadAllText( "candidates.json" );
            string tweetsJson = File.ReadAllText( "tweets.json" );

            var candidates = DjangoContainer.Unwrap<PoliticalCandidate>( candidatesJson );
            var tweets = DjangoContainer.Unwrap<PoliticalTweet>( tweetsJson );

            var mapping = new Dictionary<string, string>
            {
                { "PS", "PS" },
                { "Les Verts", "Les Verts" },
                { "PLR", "PLR" },
                { "Jeunes PLR", "PLR" },
                { "Jeunes Verts", "Les Verts" },
                { "UDC", "UDC" },
                { "Jeunes UDC", "UDC" },
                { "JUSO", "PS" },
                { "PBD", "PBD" },
                { "Jeunes PBD", "PBD" },
                { "PDC", "PDC" },
                { "Jeunes PDC", "PDC" },
                { "Vert'libéraux", "Vert'libéraux" },
                { "POP et solidaritéS", null },
                { "Pirate", null },
                { "Vote blanc", null },
                { "Ecopop", null },
                { "PEV", null },
                { "Ensemble à Gauche", null },
                { "MCG", null },
                { "Jeunes MCG", null },
                { "JDC", "PDC" },
                { "CS-POP", null },
                { "PCSI", null },
                { "La Gauche", null },
                { "POP", null },
                { "Centre Gauche-PCS", null }
            };

            candidates = candidates.Where( c => mapping.ContainsKey( c.PartyName ) && mapping[c.PartyName] != null );

            foreach ( var cand in candidates )
            {
                cand.GroupedPartyName = mapping[cand.PartyName];
            }

            File.WriteAllText( "filtered_candidates.json", DjangoContainer.Wrap( "candidate", candidates ) );
        }
    }
}