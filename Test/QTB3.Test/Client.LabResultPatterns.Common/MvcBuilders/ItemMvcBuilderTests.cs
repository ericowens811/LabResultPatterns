
using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.MvcBuilders;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using Xamarin.Forms;

namespace QTB3.Test.Client.LabResultPatterns.Common.MvcBuilders
{
    public class ItemMvcBuilderMocks<TItem>
        where TItem : class, IEntity
    {
        public Mock<ILrpNavigation> LrpNavigation { get; set; }
        public Mock<Func<IItemViewModel<TItem>>> CreateItemViewModel { get; set; }
        public Mock<Func<IItemPage<TItem>>> CreateItemPage { get; set; }
        public Mock<Func<ILrpNavigation, IItemViewModel<TItem>, IItemPage<TItem>, IItemPageController<TItem>>> CreateItemPageController { get; set; }
        public Mock<IItemViewModel<TItem>> ItemViewModel { get; set; }
        public Mock<IItemPage<TItem>> ItemPage { get; set; }
        public Mock<IItemPageController<TItem>> ItemPageController { get; set; }

        public ItemMvcBuilderMocks()
        {
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
            CreateItemViewModel = new Mock<Func<IItemViewModel<TItem>>>(MockBehavior.Strict);
            CreateItemPage = new Mock<Func<IItemPage<TItem>>>(MockBehavior.Strict);
            CreateItemPageController =
                new Mock<Func<ILrpNavigation, IItemViewModel<TItem>, IItemPage<TItem>, IItemPageController<TItem>>>(MockBehavior.Strict);
            ItemViewModel = new Mock<IItemViewModel<TItem>>(MockBehavior.Strict);
            ItemPage = new Mock<IItemPage<TItem>>(MockBehavior.Strict);
            ItemPageController = new Mock<IItemPageController<TItem>>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class ItemMvcBuilderTests
    {
        [Test]
        [Category("ItemMvcBuilder")]
        public void Constructor()
        {
            var createItemViewModel = new Mock<Func<IItemViewModel<Uom>>>().Object;
            var createItemPage = new Mock<Func<IItemPage<Uom>>>().Object;
            var createItemPageController = new Mock<Func<ILrpNavigation, IItemViewModel<Uom>, IItemPage<Uom>, IItemPageController<Uom>>>().Object;

            ConstructorTests<ItemMvcBuilder<Uom>>
                .For
                (
                    typeof(Func<IItemViewModel<Uom>>),
                    typeof(Func<IItemPage<Uom>>),
                    typeof(Func<ILrpNavigation, IItemViewModel<Uom>, IItemPage<Uom>, IItemPageController<Uom>>)
                )
                .Fail(new object[] { null, createItemPage, createItemPageController }, typeof(ArgumentNullException), "Null createItemViewModel.")
                .Fail(new object[] { createItemViewModel, null, createItemPageController }, typeof(ArgumentNullException), "Null createItemPage.")
                .Fail(new object[] { createItemViewModel, createItemPage, null }, typeof(ArgumentNullException), "Null createItemPageController.")
                .Succeed(new object[] { createItemViewModel, createItemPage, createItemPageController }, "Constructor args valid")
                .Assert();
        }

        public ItemMvcBuilder<TItem> BuildBuilder<TItem>(ItemMvcBuilderMocks<TItem> mocks)
        where TItem : class, IEntity
        {
            return new ItemMvcBuilder<TItem>
            (
                mocks.CreateItemViewModel.Object,
                mocks.CreateItemPage.Object,
                mocks.CreateItemPageController.Object
            );
        }

        public void Setup<TItem>(ItemMvcBuilderMocks<TItem> mocks, TItem item = null)
            where TItem : class, IEntity
        {
            mocks.CreateItemViewModel.Setup(f => f()).Returns(mocks.ItemViewModel.Object);
            mocks.ItemPage.Setup(i => i.InitializePage());
            mocks.ItemPage.SetupSet(i => i.BindingContext = mocks.ItemViewModel.Object);
            mocks.CreateItemPage.Setup(f => f()).Returns(mocks.ItemPage.Object);
            mocks.LrpNavigation.Setup(n => n.PushAsync(mocks.ItemPage.Object as ContentPage)).Returns(Task.CompletedTask);
            if (item != null)
            {
                mocks.ItemPageController.Setup(c => c.InitializeEditAsync(item)).Returns(Task.CompletedTask);
            }
            else
            {
                mocks.ItemPageController.Setup(c => c.InitializeAddAsync()).Returns(Task.CompletedTask);
            }
            mocks.CreateItemPageController.Setup(f => f(mocks.LrpNavigation.Object, mocks.ItemViewModel.Object, mocks.ItemPage.Object)).Returns(mocks.ItemPageController.Object);
        }

        [Test]
        [Category("ItemMvcBuilder")]
        public async Task BuildAdd()
        {
            var mocks = new ItemMvcBuilderMocks<Uom>();
            Setup(mocks);
            var builder = BuildBuilder<Uom>(mocks);
            await builder.BuildAddAsync(mocks.LrpNavigation.Object);
        }

        [Test]
        [Category("ItemMvcBuilder")]
        public async Task BuildEdit()
        {
            var uom = new Uom();
            var mocks = new ItemMvcBuilderMocks<Uom>();
            Setup(mocks, uom);
            var builder = BuildBuilder<Uom>(mocks);
            await builder.BuildEditAsync(mocks.LrpNavigation.Object, uom);
        }


    }
}
