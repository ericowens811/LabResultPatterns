using System;
using System.Collections.Generic;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Common.Utilities
{
    public class PageLinksFormatter: IPageLinksFormatter
    {
        public string GetLinks<TItem>(string urlBase, IPage<TItem> page)
        {
            // always return self link
            var links = new List<string> { GetLink(urlBase, RelTypes.self, page.SearchText, page.Skip, page.Take) };
            if (page.TotalCount > page.Take)
            {
                links.Add(GetLink(urlBase, RelTypes.first, page.SearchText, 0, page.Take));

                var lastPageSkip = (page.TotalCount / page.Take - 1) * page.Take;
                if (page.TotalCount % page.Take != 0) lastPageSkip += page.Take;
                links.Add(GetLink(urlBase, RelTypes.last, page.SearchText, lastPageSkip, page.Take));

                if (page.Skip > 0)
                {
                    var prevPageSkip = page.Skip - page.Take > 0 ? page.Skip - page.Take : 0;
                    links.Add(GetLink(urlBase, RelTypes.prev, page.SearchText, prevPageSkip, page.Take));
                }

                if (page.Skip < page.TotalCount - page.Take)
                {
                    links.Add(GetLink(urlBase, RelTypes.next, page.SearchText, page.Skip + page.Take, page.Take));
                }
            }
            return string.Join(", ", links);
        }

        private string GetLink(string urlBase, RelTypes rel, string searchText, int skip, int take)
        {
            var pageUri = $"{urlBase}?searchText={searchText}&skip={skip}&take={take}";
            return $"<{pageUri}>; rel={rel.ToString()}";
        }
    }
}
