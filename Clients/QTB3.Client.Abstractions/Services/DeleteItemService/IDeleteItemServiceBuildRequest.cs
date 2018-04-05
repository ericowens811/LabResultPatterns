
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.DeleteItemService
{
    public interface IDeleteItemServiceBuildRequest
    {
        Task DeleteItemAsync(string url);
    }
}
