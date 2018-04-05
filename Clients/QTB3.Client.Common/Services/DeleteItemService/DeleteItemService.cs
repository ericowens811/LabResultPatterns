using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.DeleteItemService;

namespace QTB3.Client.Common.Services.DeleteItemService
{
    public class DeleteItemService<TItem> : IDeleteItemService<TItem>
    {
        private readonly IDeleteItemService<TItem> _next;

        public DeleteItemService(IDeleteItemService<TItem> next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task DeleteItemAsync(int id)
        {
            if(id<1) throw new ArgumentOutOfRangeException(nameof(id));
            await _next.DeleteItemAsync(id);
        }
    }
}
