using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Common.Services.SaveItemService
{
    public class SaveItemService<TItem> : ISaveItemService<TItem>
        where TItem: class, IEntity
    {
        private readonly ISaveItemService<TItem> _next;

        public SaveItemService(ISaveItemService<TItem> next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task SaveItemAsync(TItem item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            await _next.SaveItemAsync(item);
        }
    }
}
