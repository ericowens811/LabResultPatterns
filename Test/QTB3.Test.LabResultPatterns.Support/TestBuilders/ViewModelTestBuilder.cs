using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Test.LabResultPatterns.Support.TestBuilders
{
    public class ViewModelTestBuilder<TItem> where TItem : class, IEntity
    {
        protected Queue<ICollectionPageData<TItem>> PreviousCollectionPageData;

        public string PageTitle;
        public Mock<IReadPageService<TItem>> ReadPageService;
        public Mock<IReadPageServiceNewPage<TItem>> NewPageService;
        public Mock<IDeleteItemService<TItem>> DeleteService;

        public Mock<IReadItemService<TItem>> ReadItemService;
        public Mock<ISaveItemService<TItem>> SaveItemService;

        private readonly MockSequence _serviceSequence;

        public ViewModelTestBuilder()
        {
            PreviousCollectionPageData = new Queue<ICollectionPageData<TItem>>();
            _serviceSequence = new MockSequence();
        }

        public static implicit operator UomItemViewModel(ViewModelTestBuilder<TItem> builder)
        {
            return new UomItemViewModel
            (
                builder.SaveItemService.Object as ISaveItemService<Uom>,
                builder.ReadItemService.Object as IReadItemService<Uom>
            );
        }

        public static implicit operator CollectionPageViewModel<TItem>(ViewModelTestBuilder<TItem> builder)
        {
            return new CollectionPageViewModel<TItem>
            (
               builder.PageTitle,
               builder.ReadPageService.Object,
               builder.NewPageService.Object,
               builder.DeleteService.Object
            );
        }

        public ViewModelTestBuilder<TItem> SetConstructor_Title(string title)
        {
            PageTitle = title;
            return this;
        }

        public ViewModelTestBuilder<TItem> NewPageService_NotCalled()
        {
            if (NewPageService == null)
            {
                NewPageService = new Mock<IReadPageServiceNewPage<TItem>>(MockBehavior.Strict);
            }
            else
            {
                throw new Exception("NewPageServiceNotCalled requested after call setups.");
            }
            return this;
        }

        public ViewModelTestBuilder<TItem> ReadPageService_NotCalled()
        {
            if (ReadPageService == null)
            {
                ReadPageService = new Mock<IReadPageService<TItem>>(MockBehavior.Strict);
            }
            else
            {
                throw new Exception("ReadPageServiceNotCalled requested after call setups.");
            }
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_NewPageService_ReadPageAsync(string searchText, ICollectionPageData<TItem> pageData)
        {
            PreviousCollectionPageData.Enqueue(pageData);
            if (NewPageService == null) NewPageService = new Mock<IReadPageServiceNewPage<TItem>>(MockBehavior.Strict);
            NewPageService
                .InSequence(_serviceSequence)
                .Setup(c => c.ReadPageAsync(searchText))
                .ReturnsAsync(pageData);
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_ReadPageService_PageForwardAsync(ICollectionPageData<TItem> pageData)
        {
            PreviousCollectionPageData.Enqueue(pageData);
            var previousData = PreviousCollectionPageData.Dequeue();
            if (ReadPageService == null) ReadPageService = new Mock<IReadPageService<TItem>>(MockBehavior.Strict);
            ReadPageService
                .InSequence(_serviceSequence)
                .Setup(c => c.ReadPageAsync(previousData.Links[RelTypes.next]))
                .ReturnsAsync(pageData);
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_ReadPageService_PageBackAsync(ICollectionPageData<TItem> pageData)
        {
            PreviousCollectionPageData.Enqueue(pageData);
            var previousData = PreviousCollectionPageData.Dequeue();
            if (ReadPageService == null) ReadPageService = new Mock<IReadPageService<TItem>>(MockBehavior.Strict);
            ReadPageService
                .InSequence(_serviceSequence)
                .Setup(c => c.ReadPageAsync(previousData.Links[RelTypes.prev]))
                .ReturnsAsync(pageData);
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_ReadPageService_RefreshCurrentPageAsync(ICollectionPageData<TItem> pageData)
        {
            PreviousCollectionPageData.Enqueue(pageData);
            var previousData = PreviousCollectionPageData.Dequeue();
            if (ReadPageService == null) ReadPageService = new Mock<IReadPageService<TItem>>(MockBehavior.Strict);
            ReadPageService
                .InSequence(_serviceSequence)
                .Setup(c => c.ReadPageAsync(previousData.Links[RelTypes.self]))
                .ReturnsAsync(pageData);
            return this;
        }

        public ViewModelTestBuilder<TItem> DeleteService_NotCalled()
        {
            if (DeleteService == null)
            {
                DeleteService = new Mock<IDeleteItemService<TItem>>(MockBehavior.Strict);
            }
            else
            {
                throw new Exception("DeleteServiceNotCalled requested after call setups.");
            }
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_DeleteService_DeleteItemAsync
        (
            int id
        )
        {
            if (DeleteService == null) DeleteService = new Mock<IDeleteItemService<TItem>>(MockBehavior.Strict);
            DeleteService
                .InSequence(_serviceSequence)
                .Setup(o => o.DeleteItemAsync(id)).Returns(Task.CompletedTask);
            return this;
        }

        //// ------------------------------------------------------------------------------------------

        public ViewModelTestBuilder<TItem> ReadItemService_NotCalled()
        {
            if (ReadItemService == null)
            {
                ReadItemService = new Mock<IReadItemService<TItem>>(MockBehavior.Strict);
            }
            else
            {
                throw new Exception("ReadItemServiceNotCalled requested after call setups.");
            }
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_ReadItemService_ReadItemAsync(TItem item)
        {
            if (ReadItemService == null) ReadItemService = new Mock<IReadItemService<TItem>>(MockBehavior.Strict);
            ReadItemService
                .InSequence(_serviceSequence)
                .Setup(o => o.ReadItemAsync(item.Id)).ReturnsAsync(item);
            return this;
        }

        public ViewModelTestBuilder<TItem> SaveItemService_NotCalled()
        {
            if (SaveItemService == null)
            {
                SaveItemService = new Mock<ISaveItemService<TItem>>(MockBehavior.Strict);
            }
            else
            {
                throw new Exception("SaveItemServiceNotCalled requested after call setups.");
            }
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_SaveItemService_SaveItemAsync_Throws
        (
            TItem item,
            Func<TItem, TItem, bool> equals,
            Exception exception
        )
        {
            if (SaveItemService == null) SaveItemService = new Mock<ISaveItemService<TItem>>(MockBehavior.Strict);
            SaveItemService
            .InSequence(_serviceSequence)
            .Setup
            (
                o => o.SaveItemAsync
                (
                    It.Is<TItem>(u => equals(item, u))
                )
            ).Throws(exception);
            return this;
        }

        public ViewModelTestBuilder<TItem> Then_SaveItemService_SaveItemAsync
        (
            TItem item,
            Func<TItem, TItem, bool> equals
        )
        {
            if (SaveItemService == null) SaveItemService = new Mock<ISaveItemService<TItem>>(MockBehavior.Strict);
            SaveItemService
            .InSequence(_serviceSequence)
            .Setup
            (
                o => o.SaveItemAsync
                (
                    It.Is<TItem>(u => equals(item, u))
                )
            ).Returns(Task.CompletedTask);
            return this;
        }

    }
}
