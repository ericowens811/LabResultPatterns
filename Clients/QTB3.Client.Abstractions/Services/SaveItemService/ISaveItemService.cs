
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.SaveItemService
{
    public interface ISaveItemService<TItem>
    {
        Task SaveItemAsync(TItem item);
    }
}
