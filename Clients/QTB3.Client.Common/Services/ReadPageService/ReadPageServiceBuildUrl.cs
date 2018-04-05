using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.Services.UrlBuilding;

namespace QTB3.Client.Common.Services.ReadPageService
{
    public class ReadPageServiceBuildUrl<TItem> : IReadPageServiceNewPage<TItem>
    {
        private readonly IPageUrlBuilder<TItem> _pageUrlBuilder;
        private readonly IReadPageService<TItem> _next;

        public ReadPageServiceBuildUrl
        (
            IPageUrlBuilder<TItem> pageUrlBuilder,
            IReadPageService<TItem> next
        )
        {
            _pageUrlBuilder = pageUrlBuilder ?? throw new ArgumentNullException(nameof(pageUrlBuilder));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<ICollectionPageData<TItem>> ReadPageAsync(string searchText)
        {
            var url = _pageUrlBuilder.Build(searchText);
            return await _next.ReadPageAsync(url);
        }
    }
}
