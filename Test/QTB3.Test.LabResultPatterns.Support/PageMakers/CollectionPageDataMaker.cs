using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using QTB3.Api.Common.Utilities;
using QTB3.Client.Common.Constants;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.Data;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;

namespace QTB3.Test.LabResultPatterns.Support.PageMakers
{
    public static class CollectionPageDataMaker
    {
        public static CollectionPageData<T> GetExpectedPage<T>
        (
            Page<T> expectedPage,
            string baseUrl,
            RelTypes removeRel = RelTypes.notfound
        )
        where T : IEntity
        {
            var links = new PageLinksFormatter().GetLinks(baseUrl, expectedPage);

            if (removeRel != RelTypes.notfound)
            {
                var linksArray = links.Split(',');
                var linksList = new List<string>();
                foreach (var link in linksArray)
                {
                    var match = Regex.Match(link, ClientConstants.LinksPattern);
                    var url = match.Groups[ClientConstants.UrlGroup].Value;
                    var rel = match.Groups[ClientConstants.RelGroup].Value;
                    Enum.TryParse(rel, out RelTypes relEnum);
                    if (relEnum != removeRel)
                    {
                        linksList.Add(link);
                    }
                }
                links = string.Join(", ", linksList);
            }

            return new CollectionPageData<T>(expectedPage.Items, links);
        }
    }
}
