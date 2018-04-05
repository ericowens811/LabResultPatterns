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
    public class CollectionMvcBuilder<TItem> : ICollectionMvcBuilder<TItem>
    where TItem: class, IEntity
    {
        private readonly Func<string, ICollectionPageViewModel<TItem>> _createViewModel;
        private readonly Func<ICollectionPage<TItem>> _createPage;
        private readonly Func<ILrpNavigation, ICollectionPageViewModel<TItem>, ICollectionPage<TItem>, ICollectionPageController<TItem>> _createPageController;

        public CollectionMvcBuilder
        (
            Func<string, ICollectionPageViewModel<TItem>> createViewModel,
            Func<ICollectionPage<TItem>> createPage,
            Func<ILrpNavigation, ICollectionPageViewModel<TItem>, ICollectionPage<TItem>, ICollectionPageController<TItem>> createPageController
        )
        {
            _createViewModel = createViewModel ?? throw new ArgumentNullException(nameof(createViewModel));
            _createPage = createPage ?? throw new ArgumentNullException(nameof(createPage));
            _createPageController = createPageController ?? throw new ArgumentNullException(nameof(createPageController));
        }

        public async Task BuildAsync(ILrpNavigation navigation, string pageTitle)
        {
            var viewModel = _createViewModel(pageTitle);
            var page = _createPage();
            page.InitializePage();
            page.BindingContext = viewModel;
            _createPageController
            (
                navigation,
                viewModel,
                page
            );
            await navigation.PushAsync(page as ContentPage);
        }
    }
}
