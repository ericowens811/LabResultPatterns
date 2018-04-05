
using System.Text.RegularExpressions;

namespace QTB3.Test.LabResultPatterns.Support.Links
{
    public class LinkTestsBase
    {
        public string GetUrlString(string[] links, string rel)
        {
            foreach (var link in links)
            {
                var relMatch = Regex.Match(link, @"rel=(?'rel'.*)$");
                if (relMatch.Groups["rel"].Value == rel)
                {
                    var urlMatch = Regex.Match(link, @"\<(?'url'.*)\>");
                    return urlMatch.Groups["url"].Value;
                }
            }
            return null;
        }
    }
}
