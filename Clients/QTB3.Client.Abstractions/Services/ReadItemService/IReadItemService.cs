using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.ReadItemService
{
    public interface IReadItemService<TItem>
    {
        Task<TItem> ReadItemAsync(int id);
    }
}
