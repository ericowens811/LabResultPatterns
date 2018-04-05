using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Common.Services.SaveItemService
{
    public class SaveItemServiceBuildUrl<TItem> : ISaveItemService<TItem>
        where TItem : class, IEntity
    {
        private readonly IItemUrlBuilder<TItem> _urlBuilder;
        private readonly ISaveItemServiceBuildRequest<TItem> _next;

        public SaveItemServiceBuildUrl
        (
            IItemWriteUrlBuilder<TItem> urlBuilder,
            ISaveItemServiceBuildRequest<TItem> next
        )
        {
            _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task SaveItemAsync(TItem item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            var url = item.Id == 0 ? _urlBuilder.Build() : _urlBuilder.Build(item.Id);
            await _next.SaveItemAsync(url, item);
        }
    }
}
