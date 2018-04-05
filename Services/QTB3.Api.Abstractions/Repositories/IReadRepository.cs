using System.Threading.Tasks;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Abstractions.Repositories
{
    public interface IReadRepository<TItem>
    {
        Task<IPage<TItem>> GetAsync(string searchText, int skip, int take);
        Task<TItem> GetAsync(int id);
    }
}
