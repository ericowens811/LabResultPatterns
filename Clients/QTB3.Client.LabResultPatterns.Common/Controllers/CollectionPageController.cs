using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Model.Abstractions;

namespace QTB3.Client.LabResultPatterns.Common.Controllers
{
    public class CollectionPageController<TItem> : ICollectionPageController<TItem>
        where TItem: class, IEntity
    {
        private readonly ILrpNavigation _navigation;
        private readonly ICollectionPageViewModel<TItem> _collectionPageViewModel;
        private readonly ICollectionPage<TItem> _collectionPage;
        private readonly IItemMvcBuilder<TItem> _itemMvcBuilder;
        private bool _isEditing = false;

        public CollectionPageController
        (
            ILrpNavigation navigation,
            ICollectionPageViewModel<TItem> collectionPageViewModel,
            ICollectionPage<TItem> collectionPage,
            IItemMvcBuilder<TItem> itemMvcBuilder
        )
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _collectionPageViewModel = collectionPageViewModel ?? throw new ArgumentNullException(nameof(collectionPageViewModel));
            _collectionPage = collectionPage ?? throw new ArgumentNullException(nameof(collectionPage));
            _itemMvcBuilder = itemMvcBuilder ?? throw new ArgumentNullException(nameof(itemMvcBuilder));
            WirePage();
        }

        private void WirePage()
        {
            _collectionPage.OnAppearingEvent += OnAppearingAsync;
            _collectionPage.BackEvent += BackAsync;
            _collectionPage.AddItemEvent += AddItemAsync;
            _collectionPage.EditItemSelectedEvent += EditItemSelectedAsync;
            _collectionPage.DeleteItemSelectedEvent += DeleteItemSelectedAsync;
            _collectionPage.PageForwardEvent += PageForwardAsync;
            _collectionPage.PageBackEvent += PageBackAsync;
            _collectionPage.SearchRequestedEvent += SearchRequestedAsync;
        }

        private async void OnAppearingAsync(TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            if (_isEditing)
            {
                _isEditing = false;
                await _collectionPageViewModel.RefreshAsync();
            }
            else
            {
                await _collectionPageViewModel.SearchAsync(string.Empty);
            }
            tcs.SetResult(null);
        }


        private async void BackAsync(TaskCompletionSource<object> tcs)
        {
            await _navigation.PopAsync();
            tcs.SetResult(null);
        }

        private async void AddItemAsync(TaskCompletionSource<object> tcs)
        {
            _isEditing = true;
            await _itemMvcBuilder.BuildAddAsync(_navigation);
            tcs.SetResult(null);
        }

        private async void EditItemSelectedAsync(TItem item, TaskCompletionSource<object> tcs)
        {
            _isEditing = true;
            // COULD THROW
            await _itemMvcBuilder.BuildEditAsync(_navigation, item);
            tcs.SetResult(null);
        }

        private async void DeleteItemSelectedAsync(TItem item, TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            await _collectionPageViewModel.DeleteAsync(item);
            tcs.SetResult(null);
        }

        private async void PageForwardAsync(TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            await _collectionPageViewModel.PageForwardAsync();
            tcs.SetResult(null);
        }

        private async void PageBackAsync(TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            await _collectionPageViewModel.PageBackAsync();
            tcs.SetResult(null);
        }

        private async void SearchRequestedAsync(string searchText, TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            await _collectionPageViewModel.SearchAsync(searchText);
            tcs.SetResult(null);
        }
    }
}
