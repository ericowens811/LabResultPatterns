using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Common.Constants;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Common.Services.Data
{
    public class CollectionPageData<TItem> : ICollectionPageData<TItem>
    {
        public IImmutableList<TItem> Items{ get; }
        public bool IsPaging { get; private set; }
        public bool HasForwardPages { get; private set; }
        public bool HasBackPages { get; private set; }
        public string PagingText { get; private set; }
        public IImmutableDictionary<RelTypes, string> Links { get; private set; }

        public CollectionPageData
        (
            IImmutableList<TItem> items,
            string links
        )
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            if(links == null) throw new ArgumentNullException(nameof(links));
            Updatelinks(links);
            UpdateFlags();
            if (IsPaging)
            {
                UpdateText();
            }
        }

        private void Updatelinks(string links)
        {
            var dictionary = new Dictionary<RelTypes, string>();
            var linksArray = links.Split(',');
            foreach (var link in linksArray)                                        
            {
                var match = Regex.Match(link, ClientConstants.LinksPattern);
                var url = match.Groups[ClientConstants.UrlGroup].Value;
                var rel = match.Groups[ClientConstants.RelGroup].Value;
                Enum.TryParse(rel, out RelTypes relEnum);
                if (relEnum == RelTypes.notfound)
                {
                    throw new ArgumentOutOfRangeException(nameof(rel));
                }
                dictionary.Add(relEnum, url);
            }
            Links = dictionary.ToImmutableDictionary();
        }

        private void UpdateFlags()
        {
            IsPaging = Links.Count > 1 ? true : false;
            HasForwardPages = Links.ContainsKey(RelTypes.next) ? true : false;
            HasBackPages = Links.ContainsKey(RelTypes.prev) ? true : false;
        }

        private void UpdateText()
        {
            var currentPageMatch = Regex.Match(Links[RelTypes.self], ClientConstants.SkipTakePattern);
            var currentPageSkip = int.Parse(currentPageMatch.Groups[ClientConstants.SkipGroup].Value);
            var currentPageTake = int.Parse(currentPageMatch.Groups[ClientConstants.TakeGroup].Value);
            var lastPageMatch = Regex.Match(Links[RelTypes.last], ClientConstants.SkipTakePattern);
            var lastPageSkip = int.Parse(lastPageMatch.Groups[ClientConstants.SkipGroup].Value);
            var currentPageNumber = currentPageSkip / currentPageTake + 1;
            var pageCount = lastPageSkip / currentPageTake + 1;
            PagingText = $"On page {currentPageNumber} of {pageCount}";
        }
    }
}
