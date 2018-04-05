
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.ViewModels
{
    public interface IItemViewModel<TItem>
    {
        Task GetItemAsync(int id);
        Task SaveAsync();
    }
}
