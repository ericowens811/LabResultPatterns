using System.Collections.ObjectModel;
using System.Threading.Tasks;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Abstractions.ViewModels
{
    public interface ICollectionPageViewModel<TItem> 
        where TItem : class, IEntity
    {
        string Title { get; set; }
        ObservableCollection<TItem> Items { get; }
        bool IsPaging { get; }
        string PagingText { get; }
        bool HasForwardPages { get; }
        bool HasBackPages { get; }

        Task DeleteAsync(TItem item);
        Task RefreshAsync();
        Task SearchAsync(string searchText);
        Task PageBackAsync();
        Task PageForwardAsync();
    }
}
