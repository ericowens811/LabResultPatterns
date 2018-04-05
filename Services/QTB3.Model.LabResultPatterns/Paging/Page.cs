using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using QTB3.Model.Abstractions;

namespace QTB3.Model.LabResultPatterns.Paging
{
    public class Page<TItem> : IPage<TItem>
    {
        [JsonConstructor]
        public Page
        (
            string searchText,
            int totalCount,
            int skip,
            int take,
            List<TItem> items
        )
        {
            SearchText = searchText;
            TotalCount = totalCount;
            Skip = skip;
            Take = take;
            Items = items.ToImmutableList();
        }

        public Page() { }

        public string SearchText { get; }
        public int TotalCount { get; }
        public int Skip { get; }
        public int Take { get; }
        public IImmutableList<TItem> Items { get; }
    }
}
