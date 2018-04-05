using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services.ReadPageService;

namespace QTB3.Client.Common.Services.ReadPageService
{
    public class ReadPageService<TItem> : IReadPageService<TItem>
    {
        private readonly IReadPageService<TItem> _next;

        public ReadPageService(IReadPageService<TItem> next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<ICollectionPageData<TItem>> ReadPageAsync(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            return await _next.ReadPageAsync(url);
        }
    }
}
