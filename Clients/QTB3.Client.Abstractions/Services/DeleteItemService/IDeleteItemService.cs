
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.DeleteItemService
{
    public interface IDeleteItemService<TItem>
    {
        Task DeleteItemAsync(int id);
    }
}
