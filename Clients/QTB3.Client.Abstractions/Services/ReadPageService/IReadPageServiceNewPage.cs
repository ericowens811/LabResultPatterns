using System.Threading.Tasks;
using QTB3.Client.Abstractions.Paging;

namespace QTB3.Client.Abstractions.Services.ReadPageService
{
    public interface IReadPageServiceNewPage<TItem>
    {
        Task<ICollectionPageData<TItem>> ReadPageAsync(string searchText);
    }
}
