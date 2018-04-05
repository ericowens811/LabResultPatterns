
using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Controllers;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace QTB3.Test.Client.LabResultPatterns.Common.Controllers
{
    public class ItemPageControllerMocks<TItem>
    where TItem : class, IEntity
    {
        public Mock<ILrpNavigation> LrpNavigation { get; set; }
        public Mock<IItemViewModel<TItem>> ViewModel { get; set; }

        public ItemPageControllerMocks()
        {
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
            ViewModel = new Mock<IItemViewModel<TItem>>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class ItemPageControllerTests
    {
        [Test]
        [Category("ItemPageController")]
        public void Constructor()
        {
            var lrpNavigation = new Mock<ILrpNavigation>().Object;
            var itemPageViewModel = new Mock<IItemViewModel<Uom>>().Object;
            var itemPage = new Mock<IItemPage<Uom>>().Object;

            ConstructorTests<ItemPageController<Uom>>
                .For
                (
                    typeof(ILrpNavigation),
                    typeof(IItemViewModel<Uom>),
                    typeof(IItemPage<Uom>)
                )
                .Fail(new object[] { null, itemPageViewModel, itemPage }, typeof(ArgumentNullException), "Null lrpNavigation.")
                .Fail(new object[] { lrpNavigation, null, itemPage }, typeof(ArgumentNullException), "Null itemViewModel")
                .Fail(new object[] { lrpNavigation, itemPageViewModel, null }, typeof(ArgumentNullException), "Null itemPage")
                .Succeed(new object[] { lrpNavigation, itemPageViewModel, itemPage }, "Constructor args valid")
                .Assert();
        }

        public ItemPageController<TItem> BuildController<TItem>(ItemPageControllerMocks<TItem> Mocks, IItemPage<TItem> page)
        where TItem : class, IEntity
        {
            return new ItemPageController<TItem>
            (
                Mocks.LrpNavigation.Object,
                Mocks.ViewModel.Object,
                page
            );
        }

        [Test]
        [Category("ItemPageController")]
        public async Task BackButtonPressed()
        {
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage(); 
            page.InitializePage();
            mocks.LrpNavigation.Setup(n => n.PopAsync()).ReturnsAsync(page);
            BuildController(mocks, page);
            await page.OnBackButtonPressedAsync();
        }

        [Test]
        [Category("ItemPageController")]
        public async Task SaveClicked()
        {
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage();
            page.InitializePage();
            var sequence = new MockSequence();
            mocks.ViewModel.InSequence(sequence).Setup(v => v.SaveAsync()).Returns(Task.CompletedTask);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PopAsync()).ReturnsAsync(page);
            BuildController(mocks, page);
            await page.SaveToolbarItemClickedAsync();
        }

        [Test]
        [Category("ItemPageController")]
        public async Task SaveClicked_BadRequest()
        {
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage();
            page.InitializePage();
            mocks.ViewModel.Setup(v => v.SaveAsync()).Throws(new BadRequestHttpException());
            BuildController(mocks, page);
            await page.SaveToolbarItemClickedAsync();
        }

        [Test]
        [Category("ItemPageController")]
        public async Task InitializeAdd()
        {
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage();
            page.InitializePage();
            mocks.ViewModel.Setup(v => v.GetItemAsync(0)).Returns(Task.CompletedTask);
            var controller = BuildController(mocks, page);
            await controller.InitializeAddAsync();
        }

        [Test]
        [Category("ItemPageController")]
        public async Task InitializeEdit_Item_Null()
        {
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage();
            page.InitializePage();
            var controller = BuildController(mocks, page);
            Assert.ThrowsAsync<ArgumentNullException>
            (
                async() => await controller.InitializeEditAsync(null)
            );
        }

        [Test]
        [Category("ItemPageController")]
        public async Task InitializeEdit()
        {
            var uom = new Uom().WithId(1001);
            var mocks = new ItemPageControllerMocks<Uom>();
            MockForms.Init();
            var page = new UomItemPage();
            page.InitializePage();
            mocks.ViewModel.Setup(v => v.GetItemAsync(1001)).Returns(Task.CompletedTask);
            var controller = BuildController(mocks, page);
            await controller.InitializeEditAsync(uom);
        }


    }
}
