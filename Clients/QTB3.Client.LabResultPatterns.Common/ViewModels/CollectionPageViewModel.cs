using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Model.Abstractions;

namespace QTB3.Client.LabResultPatterns.Common.ViewModels
{
    public class CollectionPageViewModel<TItem> : ViewModelBase, ICollectionPageViewModel<TItem> 
        where TItem : class, IEntity
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<TItem> _items;
        public ObservableCollection<TItem> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private string _pagingText = string.Empty;
        public string PagingText
        {
            get => _pagingText;
            set => SetProperty(ref _pagingText, value);
        }

        private bool _isPaging;
        public bool IsPaging
        {
            get => _isPaging;
            set => SetProperty(ref _isPaging, value);
        }

        private bool _hasForwardPages;
        public bool HasForwardPages
        {
            get => _hasForwardPages;
            set => SetProperty(ref _hasForwardPages, value);
        }

        private bool _hasBackPages;
        public bool HasBackPages
        {
            get => _hasBackPages;
            set => SetProperty(ref _hasBackPages, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private readonly IReadPageService<TItem> _readPageService;
        private readonly IReadPageServiceNewPage<TItem> _newPageService;
        private readonly IDeleteItemService<TItem> _deleteService;

        private ICollectionPageData<TItem> _currentPageData;

        public CollectionPageViewModel
        (
            string pageTitle,
            IReadPageService<TItem> readPageService,
            IReadPageServiceNewPage<TItem> newPageService,
            IDeleteItemService<TItem> deleteService
        )
        {
            if(string.IsNullOrWhiteSpace(pageTitle)) throw new ArgumentException(nameof(pageTitle));
            Title = pageTitle;
            _readPageService = readPageService ?? throw new ArgumentNullException(nameof(readPageService));
            _newPageService = newPageService ?? throw new ArgumentNullException(nameof(newPageService));
            _deleteService = deleteService ?? throw new ArgumentNullException(nameof(deleteService));

            Items = new ObservableCollection<TItem>();
        }

        private void UpdateItems(IImmutableList<TItem> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private void UpdatePaging(ICollectionPageData<TItem> pageData)
        {
            UpdateItems(pageData.Items);
            _currentPageData = pageData;
            IsPaging = pageData.IsPaging;
            HasForwardPages = pageData.HasForwardPages;
            HasBackPages = pageData.HasBackPages;
            PagingText = pageData.PagingText;
        }

        public async Task PageForwardAsync()
        {
            if (HasForwardPages && !IsBusy)
            {
                IsBusy = true;
                try
                {
                    UpdatePaging(
                        await _readPageService.ReadPageAsync(
                            _currentPageData.Links[RelTypes.next])); 
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task PageBackAsync()
        {
            if (HasBackPages && !IsBusy)
            {
                IsBusy = true;
                try
                {
                    UpdatePaging(await _readPageService.ReadPageAsync(_currentPageData.Links[RelTypes.prev]));
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task RefreshAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                try
                {
                    UpdatePaging(await _readPageService.ReadPageAsync(_currentPageData.Links[RelTypes.self]));
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task SearchAsync(string searchText)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                try
                {
                    UpdatePaging(await _newPageService.ReadPageAsync(searchText));
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task DeleteAsync(TItem item)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                try
                {
                    if (item == null) throw new ArgumentNullException(nameof(item));
                    await _deleteService.DeleteItemAsync(item.Id);
                    UpdatePaging(await _readPageService.ReadPageAsync(_currentPageData.Links[RelTypes.self]));
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
