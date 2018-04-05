
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Controllers
{
    public interface IItemPageController<TItem>
    {
        Task InitializeAddAsync();
        Task InitializeEditAsync(TItem item);
    }
}
