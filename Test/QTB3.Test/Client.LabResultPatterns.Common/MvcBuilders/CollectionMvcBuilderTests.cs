
using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.Services;
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
    public class BuilderMocks<TItem>
    where TItem: class, IEntity
    {
        public Mock<ILrpNavigation> LrpNavigation { get; set; }
        public Mock<Func<string, ICollectionPageViewModel<TItem>>> CreateViewModel { get; set; }
        public Mock<Func<ICollectionPage<TItem>>> CreatePage { get; set; }
        public Mock<Func<ILrpNavigation, ICollectionPageViewModel<TItem>, ICollectionPage<TItem>, ICollectionPageController<TItem>>> CreatePageController { get; set; }
        public Mock<ICollectionPageViewModel<TItem>> ViewModel;
        public Mock<ICollectionPage<TItem>> Page;
        public Mock<ICollectionPageController<TItem>> PageController;
        public Mock<Func<IToolbar, IListItem<TItem>>> CreateViewCell;

        public BuilderMocks()
        {
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
            CreateViewModel = new Mock<Func<string, ICollectionPageViewModel<TItem>>>(MockBehavior.Strict);
            CreatePage = new Mock<Func<ICollectionPage<TItem>>>(MockBehavior.Strict);
            CreatePageController = new Mock<Func<ILrpNavigation, ICollectionPageViewModel<TItem>, ICollectionPage<TItem>, ICollectionPageController<TItem>>>(MockBehavior.Strict);
            ViewModel = new Mock<ICollectionPageViewModel<TItem>>(MockBehavior.Strict);
            Page = new Mock<ICollectionPage<TItem>>(MockBehavior.Strict);
            PageController = new Mock<ICollectionPageController<TItem>>(MockBehavior.Strict);
            CreateViewCell = new Mock<Func<IToolbar, IListItem<TItem>>>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class CollectionMvcBuilderTests
    {
        [Test]
        [Category("CollectionMvcBuilder")]
        public void Constructor()
        {
            var createViewModel = new Mock<Func<string, ICollectionPageViewModel<Uom>>>().Object;
            var createPage = new Mock<Func<ICollectionPage<Uom>>>().Object;
            var createController = new Mock<Func<ILrpNavigation, ICollectionPageViewModel<Uom>, ICollectionPage<Uom>, ICollectionPageController<Uom>>>().Object;

            ConstructorTests<CollectionMvcBuilder<Uom>>
                .For
                (
                    typeof(Func<string, ICollectionPageViewModel<Uom>>),
                    typeof(Func<ICollectionPage<Uom>>),
                    typeof(Func<ILrpNavigation, ICollectionPageViewModel<Uom>, ICollectionPage<Uom>, ICollectionPageController<Uom>>)
                )
                .Fail(new object[] { null, createPage, createController }, typeof(ArgumentNullException), "Null createViewModel.")
                .Fail(new object[] { createViewModel, null, createController }, typeof(ArgumentNullException), "Null createPage.")
                .Fail(new object[] { createViewModel, createPage, null }, typeof(ArgumentNullException), "Null createController.")
                .Succeed(new object[] { createViewModel, createPage, createController }, "Constructor args valid")
                .Assert();
        }

        public CollectionMvcBuilder<TItem> BuildBuilder<TItem>(BuilderMocks<TItem> mocks)
        where TItem: class, IEntity
        {
            return new CollectionMvcBuilder<TItem>
            (
                mocks.CreateViewModel.Object,
                mocks.CreatePage.Object,
                mocks.CreatePageController.Object
            );
        }

        public void Setup<TItem>(BuilderMocks<TItem> mocks, string pageTitle)
        where TItem: class, IEntity
        {
            mocks.CreateViewModel.Setup(f => f(pageTitle)).Returns(mocks.ViewModel.Object);
            mocks.CreatePage.Setup(f => f()).Returns(mocks.Page.Object);
            mocks.CreatePageController
                .Setup(f => f(mocks.LrpNavigation.Object, mocks.ViewModel.Object, mocks.Page.Object))
                .Returns(mocks.PageController.Object);
            mocks.Page.Setup(p => p.InitializePage());
            mocks.Page.SetupSet(p => p.BindingContext = mocks.ViewModel.Object);
            mocks.LrpNavigation.Setup(n => n.PushAsync(mocks.Page.Object as ContentPage)).Returns(Task.CompletedTask);
        }

        [Test]
        [Category("CollectionMvcBuilder")]
        public async Task Build()
        {
            var pageTitle = "TheTitle";
            var mocks = new BuilderMocks<Uom>();
            Setup(mocks, pageTitle);
            var builder = BuildBuilder(mocks);

            await builder.BuildAsync(mocks.LrpNavigation.Object, pageTitle);
        }
    }
}
