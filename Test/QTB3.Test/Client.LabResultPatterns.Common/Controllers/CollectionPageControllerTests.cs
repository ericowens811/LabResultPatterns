using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Controllers;
using QTB3.Client.LabResultPatterns.Common.Views;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using Xamarin.Forms.Mocks;

namespace QTB3.Test.Client.LabResultPatterns.Common.Controllers
{
    public class CollectionPageControllerMocks<TItem>
    where TItem : class, IEntity
    {
        public Mock<ILrpNavigation> LrpNavigation { get; set; }
        public Mock<ICollectionPageViewModel<TItem>> CollectionPageViewModel { get; set; }
        public Mock<IItemMvcBuilder<TItem>> ItemMvcBuilder { get; set; }
        public Mock<Func<IToolbar, IListItem<TItem>>> CreateViewCell { get; set; }

        public CollectionPageControllerMocks()
        {
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
            CollectionPageViewModel = new Mock<ICollectionPageViewModel<TItem>>(MockBehavior.Strict);
            ItemMvcBuilder = new Mock<IItemMvcBuilder<TItem>>(MockBehavior.Strict);
            CreateViewCell = new Mock<Func<IToolbar, IListItem<TItem>>>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class CollectionPageControllerTests
    {
        [Test]
        [Category("CollectionPageController")]
        public void Constructor()
        {
            var lrpNavigation = new Mock<ILrpNavigation>().Object;
            var collectionPageViewModel = new Mock<ICollectionPageViewModel<Uom>>().Object;
            var collectionPage = new Mock<ICollectionPage<Uom>>().Object;
            var itemMvcBuilder = new Mock<IItemMvcBuilder<Uom>>().Object;

            ConstructorTests<CollectionPageController<Uom>>
                .For
                (
                    typeof(ILrpNavigation), 
                    typeof(ICollectionPageViewModel<Uom>), 
                    typeof(ICollectionPage<Uom>), 
                    typeof(IItemMvcBuilder<Uom>)
                )
                .Fail(new object[] { null, collectionPageViewModel, collectionPage, itemMvcBuilder }, typeof(ArgumentNullException), "Null lrpNavigation.")
                .Fail(new object[] { lrpNavigation, null, collectionPage, itemMvcBuilder }, typeof(ArgumentNullException), "Null collectionPageViewModel.")
                .Fail(new object[] { lrpNavigation, collectionPageViewModel, null, itemMvcBuilder }, typeof(ArgumentNullException), "Null collectionPage.")
                .Fail(new object[] { lrpNavigation, collectionPageViewModel, collectionPage, null, }, typeof(ArgumentNullException), "Null itemMvcBuilder.")
                .Succeed(new object[] { lrpNavigation, collectionPageViewModel, collectionPage, itemMvcBuilder },"Constructor args valid")
                .Assert();
        }

        public CollectionPageController<TItem> BuildController<TItem>(CollectionPageControllerMocks<TItem> mocks, CollectionPage<TItem> page)
        where TItem : class, IEntity
        {
            return new CollectionPageController<TItem>
            (
                mocks.LrpNavigation.Object,
                mocks.CollectionPageViewModel.Object,
                page,
                mocks.ItemMvcBuilder.Object
            );
        }

        public void SetupForAddEditWorkflow<TItem>(CollectionPageControllerMocks<TItem> mocks, TItem item=null)
        where TItem : class, IEntity
        {
            if (item == null)
            {
                mocks.ItemMvcBuilder.Setup(i => i.BuildAddAsync(mocks.LrpNavigation.Object))
                    .Returns(Task.CompletedTask);
            }
            else
            {
                mocks.ItemMvcBuilder.Setup(i => i.BuildEditAsync(mocks.LrpNavigation.Object, item))
                    .Returns(Task.CompletedTask);
            }
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task OnAppearingNotEditing()
        {
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.SearchAsync(string.Empty)).Returns(Task.CompletedTask);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.OnAppearingAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task OnAppearingIsEditing()
        {
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.RefreshAsync()).Returns(Task.CompletedTask);
            SetupForAddEditWorkflow(mocks);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.AddButtonClickedAsync();
            await page.OnAppearingAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task AddWorkflow()
        {
            var uom = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.RefreshAsync()).Returns(Task.CompletedTask);
            SetupForAddEditWorkflow(mocks);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.AddButtonClickedAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task EditWorkflow()
        {
            var uom = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.RefreshAsync()).Returns(Task.CompletedTask);
            SetupForAddEditWorkflow(mocks, uom);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.OnItemSelectedAsync(uom);
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task DeleteItem()
        {
            var uomToDelete = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.DeleteAsync(uomToDelete)).Returns(Task.CompletedTask);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.DeleteButtonClickedAsync();
            await page.OnItemSelectedAsync(uomToDelete);
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task PageForward()
        {
            var uomToDelete = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.PageForwardAsync()).Returns(Task.CompletedTask);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.PageForwardClickedAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task PageBack()
        {
            var uomToDelete = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            mocks.CollectionPageViewModel.Setup(v => v.PageBackAsync()).Returns(Task.CompletedTask);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            BuildController(mocks, page);

            await page.PageBackClickedAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task SearchRequested()
        {
            var uomToDelete = new Uom();
            var mocks = new CollectionPageControllerMocks<Uom>();
            MockForms.Init(); // for the page SearchBar 
            mocks.CollectionPageViewModel.Setup(v => v.SearchAsync("j")).Returns(Task.CompletedTask);
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            page.InitializePage();
            BuildController(mocks, page);
            
            page.SearchBar.Text = "j";
            await page.SearchRequestedAsync();
        }

        [Test]
        [Category("CollectionPageController")]
        public async Task Back()
        {
            var mocks = new CollectionPageControllerMocks<Uom>();
            var page = new CollectionPage<Uom>(mocks.CreateViewCell.Object);
            mocks.LrpNavigation.Setup(v => v.PopAsync()).ReturnsAsync(page);
            BuildController(mocks, page);

            await page.OnBackButtonPressedAsync();
        }

    }
}
