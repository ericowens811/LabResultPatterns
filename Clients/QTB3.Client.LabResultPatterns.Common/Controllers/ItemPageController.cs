using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Model.Abstractions;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Controllers
{
    public class ItemPageController<TItem> : IItemPageController<TItem>
        where TItem : class, IEntity
    {
        private readonly ILrpNavigation _navigation;
        private readonly IItemViewModel<TItem> _viewModel;
        private readonly IItemPage<TItem> _page;

        public ItemPageController
        (
            ILrpNavigation navigation,
            IItemViewModel<TItem> viewModel, 
            IItemPage<TItem> page
        )
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _page = page ?? throw new ArgumentNullException(nameof(page));

            WirePage();
        }

        private void WirePage()
        {
            _page.BindingContext = _viewModel;
            _page.BackButtonPressedEvent += BackButtonPressed;

            _page.OnAppearingCalledEvent += OnAppearingCalled;
            _page.SaveClickedEvent += SaveClicked;
        }

        private async void OnAppearingCalled(TaskCompletionSource<object> tcs)
        {
            tcs.SetResult(null);
        }

        private async void BackButtonPressed(TaskCompletionSource<object> tcs)
        {
            await _navigation.PopAsync();
            tcs.SetResult(null);
        }

        private async void SaveClicked(TaskCompletionSource<object> tcs)
        {
            // COULD THROW
            // If it throws LoginRequestedException,
            // simply abort the save workflow and the user will 
            // be returned to the item page
            var success = true;
            try
            {
                await _viewModel.SaveAsync();
            }
            catch (BadRequestHttpException) // catching BadRequest here indicates validation errors
            {
                success = false; 
            }

            if (success)
            {
                await _navigation.PopAsync();
            }
            tcs.SetResult(null);
        }

        public async Task InitializeAddAsync()
        {
            await _viewModel.GetItemAsync(0);
        }

        public async Task InitializeEditAsync(TItem item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            // COULD THROW
            await _viewModel.GetItemAsync(item.Id);
        }
    }
}
