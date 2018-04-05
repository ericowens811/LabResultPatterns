using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Views;
using QTB3.Model.Abstractions;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Views
{
    public partial class ItemPage<TItem> : ContentPage, IItemPage<TItem>, IOnAppearingAsync where TItem : class, IEntity
    {
        public event Action<TaskCompletionSource<object>> OnAppearingCalledEvent;
        public event Action<TaskCompletionSource<object>> BackButtonPressedEvent;
        public event Action<TaskCompletionSource<object>> SaveClickedEvent;

        public ItemPage()
        {
        }

        public void InitializePage()
        {
            CreatePage();
            WireToolbar();
        }

        protected virtual void WireToolbar()
        {
            SaveToolbarItem.Clicked += SaveToolbarItemClicked;
        }

        public async Task OnAppearingAsync()
        {
            if (OnAppearingCalledEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                OnAppearingCalledEvent.Invoke(tcs);
                await tcs.Task;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await OnAppearingAsync();
        }

        public async Task OnBackButtonPressedAsync()
        {
            if (BackButtonPressedEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                BackButtonPressedEvent.Invoke(tcs);
                await tcs.Task;
            }
        }

        protected async void OnBackButtonPressedAsyncVoid()
        {
            await OnBackButtonPressedAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            OnBackButtonPressedAsyncVoid();
            return true;
        }

        public async void SaveToolbarItemClicked(object sender, EventArgs e)
        {
            await SaveToolbarItemClickedAsync();
        }

        public async Task SaveToolbarItemClickedAsync()
        {
            if (SaveClickedEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                SaveClickedEvent.Invoke(tcs);
                await tcs.Task;
            }
        }
    }
}
