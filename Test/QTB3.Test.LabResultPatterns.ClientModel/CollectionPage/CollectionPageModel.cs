using System;
using System.Threading.Tasks;
using NUnit.Framework;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Model.Abstractions;
using QTB3.Test.LabResultPatterns.ClientModel.Abstractions;
using QTB3.Test.LabResultPatterns.ClientModel.Pagebar;

namespace QTB3.Test.LabResultPatterns.ClientModel.CollectionPage
{
    public class CollectionPageModel<TItem> : ICollectionPageInteraction<TItem> 
    where TItem: class, IEntity
    {
        private readonly Func<IReadRepository<TItem>> _getReadRepository;
        private readonly Func<IWriteRepository<TItem>> _getWriteRepository;
        private readonly PagebarModel<TItem> _pagebarModel;

        private IPage<TItem> _expectedPage;
        private int _skip;
        private const int Take = LrpConstants.PageSize;

        public CollectionPageModel
        (
            Func<IReadRepository<TItem>> getReadRepository,
            Func<IWriteRepository<TItem>> getWriteRepository
        )
        {
            _getReadRepository = getReadRepository;
            _getWriteRepository = getWriteRepository;
            _pagebarModel = new PagebarModel<TItem>();
            ClickEditOnToolbar();
            _skip = 0;
        }

        protected virtual void SetToolStateFalse()
        {
            IsDeleting = false;
            IsEditing = false;
        }

        public void ClickEditOnToolbar()
        {
            SetToolStateFalse();
            IsEditing = true;
        }

        public void ClickDeleteOnToolbar()
        {
            SetToolStateFalse();
            IsDeleting = true;
        }

        private async Task GetPage()
        {
            _expectedPage = await _getReadRepository().GetAsync(SearchText, _skip, Take);
            _pagebarModel.Update(ExpectedPage);
        }

        public void SetSearchText(string text)
        {
            SearchText = text;
        }

        public async Task Search()
        {
            await GetPage();
        }

        public TItem Select(int index)
        {
             Assert.True(_expectedPage.Items.Count > index);
             return _expectedPage.Items[index];
        }

        public async Task DeleteAsync(int index)
        {
            Assert.True(_expectedPage.Items.Count > index);
            Assert.True(IsDeleting);
            await _getWriteRepository().DeleteAsync(_expectedPage.Items[index].Id);
            await GetPage();
        }

        public async Task PageBackTappedAsync()
        {
            Assert.True(_pagebarModel.IsVisible);
            Assert.True(_pagebarModel.PageBackIsVisible);
            _skip -= Take;
            await GetPage();
        }

        public async Task PageForwardTappedAsync()
        {
            Assert.True(_pagebarModel.IsVisible);
            Assert.True(_pagebarModel.PageForwardIsVisible);
            _skip += Take;
            await GetPage();
        }

        public async Task OnReappearingAsync()
        {
            await GetPage();
        }

        public async Task OnAppearingAsync()
        {
            _skip = 0;
            await GetPage();
        }

        public string TitleText { get; set; }
        public string SearchText { get; private set; }

        public PagebarModel<TItem> ExpectedPagebar => _pagebarModel;
        public IPage<TItem> ExpectedPage => _expectedPage;
        public bool IsDeleting { get; private set; }
        public bool IsEditing { get; private set; }
    }
}
