
using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Model.Abstractions;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.MvcBuilders
{
    public class ItemMvcBuilder<TItem> : IItemMvcBuilder<TItem>
    where TItem: class, IEntity
    {
        private readonly Func<IItemViewModel<TItem>> _createItemViewModel;
        private readonly Func<IItemPage<TItem>> _createItemPage;
        private Func<ILrpNavigation, IItemViewModel<TItem>, IItemPage<TItem>, IItemPageController<TItem>> _createItemPageController;

        public ItemMvcBuilder
        (
            Func<IItemViewModel<TItem>> createItemViewModel,
            Func<IItemPage<TItem>> createItemPage,
            Func<ILrpNavigation, IItemViewModel<TItem>, IItemPage<TItem>, IItemPageController<TItem>> createItemPageController
        )
        {
            _createItemViewModel = createItemViewModel ?? throw new ArgumentNullException(nameof(createItemViewModel));
            _createItemPage = createItemPage ?? throw new ArgumentNullException(nameof(createItemPage));
            _createItemPageController = createItemPageController ?? throw new ArgumentNullException(nameof(createItemPageController));
        }

        public async Task BuildAddAsync(ILrpNavigation navigation)
        {
            var itemViewModel = _createItemViewModel();
            var itemPage = _createItemPage();
            itemPage.InitializePage();
            itemPage.BindingContext = itemViewModel;
            var itemPageController = _createItemPageController(navigation, itemViewModel, itemPage);
            await itemPageController.InitializeAddAsync();
            await navigation.PushAsync(itemPage as ContentPage);
        }

        public async Task BuildEditAsync(ILrpNavigation navigation, TItem item)
        {
            var itemViewModel = _createItemViewModel();
            var itemPage = _createItemPage();
            itemPage.InitializePage();
            itemPage.BindingContext = itemViewModel;
            var itemPageController = _createItemPageController(navigation, itemViewModel, itemPage);
            // COULD THROW
            await itemPageController.InitializeEditAsync(item);
            await navigation.PushAsync(itemPage as ContentPage);
        }
    }
}
