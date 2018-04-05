using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Common.Converters;
using QTB3.Model.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QTB3.Client.LabResultPatterns.Common.Views
{
    //
    // IOnAppearingAsync: used in TestDevice so that OnAppearingAsync can be called (emulation of page lifecycle)
    // IToolbar: used in listitems to bind icons' IsVisible
    //
    public partial class CollectionPage<TItem> : ContentPage, ICollectionPage<TItem>
        where TItem : class, IEntity
    {
        protected Func<IToolbar, IListItem<TItem>> CreateViewCell;

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        private bool _isDeleting;
        public bool IsDeleting
        {
            get => _isDeleting;
            set => SetProperty(ref _isDeleting, value);
        }

        public virtual async Task DeleteButtonClickedAsync()
        {
            IsDeleting = true;
            IsEditing = false;
        }

        public virtual async void DeleteToolbarItemClicked(object sender, EventArgs args)
        {
            await DeleteButtonClickedAsync();
        }

        public virtual async Task EditButtonClickedAsync()
        {
            IsDeleting = false;
            IsEditing = true;
        }

        public virtual async void EditToolbarItemClicked(object sender, EventArgs args)
        {
            await EditButtonClickedAsync();
        }

        public event Action<TaskCompletionSource<object>> OnAppearingEvent;
        public event Action<TaskCompletionSource<object>> AddItemEvent;
        public event Action<TItem, TaskCompletionSource<object>> EditItemSelectedEvent;
        public event Action<TItem, TaskCompletionSource<object>> DeleteItemSelectedEvent;
        public event Action<TaskCompletionSource<object>> BackEvent;
        public event Action<TaskCompletionSource<object>> PageForwardEvent;
        public event Action<TaskCompletionSource<object>> PageBackEvent;
        public event Action<string, TaskCompletionSource<object>> SearchRequestedEvent;

        public CollectionPage
        (
            Func<IToolbar, IListItem<TItem>> createViewCell
        )
        {
            CreateViewCell = createViewCell;
        }

        protected virtual ViewCell MakeViewCell()
        {
            return CreateViewCell(this) as ViewCell;
        }

        public void InitializePage()
        {
            CreatePage();
            InitializeInstanceVars();
            WirePage();
            WireToolbar();
            WireListView();
            WireSearchbar();
            WirePagebar();
        }

        protected virtual void InitializeInstanceVars()
        {
            IsDeleting = false;
            IsEditing = true;
        }

        protected virtual void WirePage()
        {
            this.SetBinding(TitleProperty, "Title");
        }

        // TODO might need to clean up the listeners in OnDisappearing(), so its all here
        protected virtual void WireToolbar()
        {
            DeleteToolbarItem.Clicked += DeleteToolbarItemClicked;
            EditToolbarItem.Clicked += EditToolbarItemClicked;
            AddToolbarItem.Clicked += AddToolbarItemClicked;
        }

        protected virtual void WireListView()
        {
            ItemsListView.SetBinding(ListView.ItemsSourceProperty, "Items");
            ItemsListView.Refreshing += SearchRequested;
            ItemsListView.ItemSelected += OnItemSelected;
            ItemsListView.ItemTemplate = new DataTemplate(MakeViewCell);
        }

        protected virtual void WireSearchbar()
        {
            SearchBar.SearchButtonPressed += SearchRequested;
        }

        protected virtual void WirePagebar()
        {
            Pagebar.SetBinding(IsVisibleProperty, "IsPaging");

            Pagebar.PageForward.SetBinding(Image.IsVisibleProperty, "HasForwardPages");
            Pagebar.PageForwardDisabled.SetBinding(Image.IsVisibleProperty, "HasForwardPages", BindingMode.Default, new InvertBooleanConverter());
            Pagebar.PageBack.SetBinding(Image.IsVisibleProperty, "HasBackPages");
            Pagebar.PageBackDisabled.SetBinding(Image.IsVisibleProperty, "HasBackPages", BindingMode.Default, new InvertBooleanConverter());

            Pagebar.TapPageBack.Tapped += PageBackTapped;
            Pagebar.TapPageForward.Tapped += PageForwardTapped;
            Pagebar.PagingLabel.SetBinding(Label.TextProperty, "PagingText");
        }

        public async Task OnAppearingAsync()
        {
            if (OnAppearingEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                OnAppearingEvent.Invoke(tcs);
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
            if (BackEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                BackEvent.Invoke(tcs);
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

        public async Task PageForwardClickedAsync()
        {
            if (PageForwardEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                PageForwardEvent.Invoke(tcs);
                await tcs.Task;
            }
        }

        protected async void PageForwardTapped(object sender, EventArgs args)
        {
            await PageForwardClickedAsync();
        }

        public async Task PageBackClickedAsync()
        {
            if (PageBackEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                PageBackEvent.Invoke(tcs);
                await tcs.Task;
            }
        }

        protected async void PageBackTapped(object sender, EventArgs args)
        {
            await PageBackClickedAsync();
        }

        public async Task SearchRequestedAsync()
        {
            if (SearchRequestedEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                SearchRequestedEvent.Invoke(SearchBar.Text, tcs);
                await tcs.Task;
            }
        }

        protected async void SearchRequested(object sender, EventArgs args)
        {
            await SearchRequestedAsync();
        }

        public async Task AddButtonClickedAsync()
        {
            if (AddItemEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                AddItemEvent.Invoke(tcs);
                await tcs.Task;
            }
        }

        protected async void AddToolbarItemClicked(object sender, EventArgs args)
        {
            await AddButtonClickedAsync();
        }

        public async Task OnItemSelectedAsync(TItem item)
        {
            if (IsEditing)
            {
                if (EditItemSelectedEvent != null)
                {
                    var tcs = new TaskCompletionSource<object>();
                    EditItemSelectedEvent.Invoke(item, tcs);
                    await tcs.Task;
                }
            }
            else if (IsDeleting)
            {
                if (DeleteItemSelectedEvent != null)
                {
                    var tcs = new TaskCompletionSource<object>();
                    DeleteItemSelectedEvent.Invoke(item, tcs);
                    await tcs.Task;
                }
            }
            // Manually deselect item
            if(ItemsListView != null) // could be null to simplify unit tests
                ItemsListView.SelectedItem = null;
        }

        // this is wired up in WireListView() above
        protected async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Deleting raises the event but returns null item
            if (!(args.SelectedItem is TItem item))
                return;
            await OnItemSelectedAsync(item);
        }

        // for the toolbar state vars
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        // test bench probes
        public IEnumerable<TItem> ListViewSourceProbe => ItemsListView.ItemsSource as IEnumerable<TItem>;
        public TemplatedItemsList<ItemsView<Cell>, Cell> ViewCellsProbe => ItemsListView.TemplatedItems;
    }
}
