using System.Threading.Tasks;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Abstractions.Services.SaveItemService
{
    public interface ISaveItemServiceBuildRequest<TItem>
        where TItem : class, IEntity
    {
        Task SaveItemAsync(string url, TItem item);
    }
}
