using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.ReadItemService;

namespace QTB3.Client.Common.Services.ReadItemService
{
    public class ReadItemService<TItem> : IReadItemService<TItem>
    {
        private readonly IReadItemService<TItem> _next;

        public ReadItemService(IReadItemService<TItem> next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<TItem> ReadItemAsync(int id)
        {
            if(id < 1) throw new ArgumentOutOfRangeException(nameof(id));
            return await _next.ReadItemAsync(id);
        }
    }
}
