using System.Threading.Tasks;

namespace QTB3.Api.Abstractions.Repositories
{
    public interface IWriteRepository<TItem>
    {
        Task<TItem> PostAsync(TItem value);
        Task PutAsync(TItem value);
        Task<bool> DeleteAsync(int id);
    }
}
