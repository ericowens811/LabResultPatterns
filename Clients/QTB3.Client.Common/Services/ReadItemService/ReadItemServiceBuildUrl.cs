using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;

namespace QTB3.Client.Common.Services.ReadItemService
{
    public class ReadItemServiceBuildUrl<TItem> : IReadItemService<TItem>
    {
        private readonly IItemUrlBuilder<TItem> _urlBuilder;
        private readonly IReadItemServiceBuildRequest<TItem> _next;

        public ReadItemServiceBuildUrl
        (
            IItemReadUrlBuilder<TItem> urlBuilder,
            IReadItemServiceBuildRequest<TItem> next
        )
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<TItem> ReadItemAsync(int id)
        {
            if(id < 1) throw new ArgumentOutOfRangeException(nameof(id));
            var url = _urlBuilder.Build(id);
            return await _next.ReadItemAsync(url);
        }
    }
}
