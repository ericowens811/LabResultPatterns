using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.ReadItemService
{
    public interface IReadItemServiceBuildRequest<TItem>
    {
        Task<TItem> ReadItemAsync(string url);
    }
}
