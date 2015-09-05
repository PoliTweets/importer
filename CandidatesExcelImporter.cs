using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Excel;
using PoliticalTweetsImporter.Models;

namespace PoliticalTweetsImporter
{
    public static class CandidatesExcelImporter
    {
        public static IReadOnlyCollection<PoliticalCandidate> LoadCandidates( string filePath )
        {
            var results = new List<PoliticalCandidate>();

            using ( var stream = File.OpenRead( filePath ) )
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader( stream );
                reader.IsFirstRowAsColumnNames = true;

                var result = reader.AsDataSet();
                foreach ( DataTable table in result.Tables.Cast<DataTable>().Where( t => t.Columns.Contains( "URL TW" ) && t.TableName != "Total" ) )
                {
                    string cantonName = table.TableName;
                    if ( cantonName == "JUBE" )
                    {
                        cantonName = "BE";
                    }

                    results.AddRange( from DataRow row in table.Rows
                                      where row["URL TW"] != DBNull.Value
                                      let handle = ExtractHandle( (string) row["URL TW"] )
                                      where handle != null
                                      let personName = ( (string) row["Nom"] ).Trim()
                                      let party = ( (string) row["Parti"] ).Trim()
                                      select new PoliticalCandidate( personName, party, cantonName, handle ) );
                }
            }

            return results;
        }

        private static string ExtractHandle( string url )
        {
            var match = Regex.Match( url, @"https://twitter\.com/(?<name>[^/\?]+)(/.*)?(\?.*)?" );
            if ( match.Success )
            {
                return match.Groups["name"].Value;
            }
            return null;
        }
    }
}