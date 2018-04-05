
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using QTB3.Model.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QTB3.Client.Abstractions.Views
{
    public interface ICollectionPage<TItem> : IOnAppearingAsync, IToolbar
    where TItem : class, IEntity
    {
        void InitializePage();

        object BindingContext { get; set; }

        event Action<TaskCompletionSource<object>> OnAppearingEvent;
        event Action<TaskCompletionSource<object>> AddItemEvent;
        event Action<TItem, TaskCompletionSource<object>> EditItemSelectedEvent;
        event Action<TItem, TaskCompletionSource<object>> DeleteItemSelectedEvent;
        event Action<TaskCompletionSource<object>> BackEvent;
        event Action<TaskCompletionSource<object>> PageForwardEvent;
        event Action<TaskCompletionSource<object>> PageBackEvent;
        event Action<string, TaskCompletionSource<object>> SearchRequestedEvent;

        Task PageForwardClickedAsync();
        Task PageBackClickedAsync();
        Task SearchRequestedAsync();
        Task DeleteButtonClickedAsync();
        Task EditButtonClickedAsync();
        Task AddButtonClickedAsync();
        Task OnItemSelectedAsync(TItem item);

        SearchBar SearchBar { get; }

        IEnumerable<TItem> ListViewSourceProbe { get; }
        TemplatedItemsList<ItemsView<Cell>, Cell> ViewCellsProbe { get; }
    }
}
