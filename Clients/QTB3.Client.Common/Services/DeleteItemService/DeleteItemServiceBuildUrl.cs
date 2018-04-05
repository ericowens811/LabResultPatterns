using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;

namespace QTB3.Client.Common.Services.DeleteItemService
{
    public class DeleteItemServiceBuildUrl<TItem> : IDeleteItemService<TItem>
    {
        private readonly IItemUrlBuilder<TItem> _urlBuilder;
        private readonly IDeleteItemServiceBuildRequest _next;

        public DeleteItemServiceBuildUrl
        (
            IItemWriteUrlBuilder<TItem> urlBuilder,
            IDeleteItemServiceBuildRequest next
        )
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task DeleteItemAsync(int id)
        {
            if(id < 1) throw new ArgumentOutOfRangeException(nameof(id));
            var url = _urlBuilder.Build(id);
            await _next.DeleteItemAsync(url);
        }
    }
}
