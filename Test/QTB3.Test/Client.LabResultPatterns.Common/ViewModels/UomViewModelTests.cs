using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.Checkers.ViewModelChecking;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.PageMakers;
using QTB3.Test.LabResultPatterns.Support.TestBuilders;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.LabResultPatterns.Common.ViewModels
{
    [TestFixture]
    public class UomViewModelTests 
    {
        [Test]
        [Category("UomViewModel")]
        public void Constructor()
        {
            var pageTitle = "Home";
            var readPageService = new Mock<IReadPageService<Uom>>().Object;
            var newPageService = new Mock<IReadPageServiceNewPage<Uom>>().Object;
            var deleteService = new Mock<IDeleteItemService<Uom>>().Object;
            ConstructorTests<CollectionPageViewModel<Uom>>
                .For(typeof(string), typeof(IReadPageService<Uom>), typeof(IReadPageServiceNewPage<Uom>),  typeof(IDeleteItemService<Uom>))
                .Fail(new object[] { string.Empty, readPageService, newPageService, deleteService }, typeof(ArgumentException), "Empty Title.")
                .Fail(new object[] { pageTitle, null, newPageService, deleteService }, typeof(ArgumentNullException), "Null readPageService.")
                .Fail(new object[] { pageTitle, readPageService, null, deleteService }, typeof(ArgumentNullException), "Null newPageService.")
                .Fail(new object[] { pageTitle, readPageService, newPageService, null }, typeof(ArgumentNullException), "Null deleteService.")
                .Succeed(new object[] { pageTitle, readPageService, newPageService, deleteService }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("UomViewModel")]
        public void ConstructorAssignsTitle()
        {
            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .NewPageService_NotCalled()
                .ReadPageService_NotCalled()
                .DeleteService_NotCalled();
            Assert.AreEqual("Uoms", viewModel.Title);
        }

        [Test]
        [Category("UomViewModel")]
        public void DeleteAsync_ItemNull()
        {
            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .NewPageService_NotCalled()
                .ReadPageService_NotCalled()
                .DeleteService_NotCalled();
            Assert.ThrowsAsync<ArgumentNullException>
            (
                async () => await viewModel.DeleteAsync(null)
            );
        }

        [Test]
        [Category("UomViewModel")]
        public async Task DeleteAsync()
        {
            var page = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 0,
                take: 20
            );
            var expectedPage = CollectionPageDataMaker.GetExpectedPage(page, "http://localhost/api");

            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .Then_NewPageService_ReadPageAsync("", expectedPage)
                .Then_DeleteService_DeleteItemAsync(1001)
                .Then_ReadPageService_RefreshCurrentPageAsync(expectedPage);

            await viewModel.SearchAsync("");
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage, viewModel, UomEqual.Check));
            await viewModel.DeleteAsync(new Uom().WithId(1001));
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage, viewModel, UomEqual.Check));
        }

        [Test]
        [Category("UomViewModel")]
        public async Task SearchAsync()
        {
            var page = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 0,
                take: 20
            );
            var expectedPage = CollectionPageDataMaker.GetExpectedPage(page, "http://localhost/api");
            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .DeleteService_NotCalled()
                .ReadPageService_NotCalled()
                .Then_NewPageService_ReadPageAsync("", expectedPage);
            await viewModel.SearchAsync("");
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage, viewModel, UomEqual.Check));
        }

        [Test]
        [Category("UomViewModel")]
        public async Task SearchAsync_PageForwardAsync()
        {
            var page1 = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 0,
                take: 20
            );
            var expectedPage1 = CollectionPageDataMaker.GetExpectedPage(page1, "http://localhost/api");

            var page2 = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 20,
                take: 20
            );
            var expectedPage2 = CollectionPageDataMaker.GetExpectedPage(page2, "http://localhost/api");

            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .DeleteService_NotCalled()
                .Then_NewPageService_ReadPageAsync("", expectedPage1)
                .Then_ReadPageService_PageForwardAsync(expectedPage2);
            await viewModel.SearchAsync("");
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage1, viewModel, UomEqual.Check));
            await viewModel.PageForwardAsync();
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage2, viewModel, UomEqual.Check));
        }

        [Test]
        [Category("UomViewModel")]
        public async Task SearchAsync_PageForwardAsync_PageBackAsync()
        {
            var page1 = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 0,
                take: 20
            );
            var expectedPage1 = CollectionPageDataMaker.GetExpectedPage(page1, "http://localhost/api");

            var page2 = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 20,
                take: 20
            );
            var expectedPage2 = CollectionPageDataMaker.GetExpectedPage(page2, "http://localhost/api");

            CollectionPageViewModel<Uom> viewModel = new ViewModelTestBuilder<Uom>()
                .SetConstructor_Title("Uoms")
                .DeleteService_NotCalled()
                .Then_NewPageService_ReadPageAsync("", expectedPage1)
                .Then_ReadPageService_PageForwardAsync(expectedPage2)
                .Then_ReadPageService_PageBackAsync(expectedPage1);
            await viewModel.SearchAsync("");
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage1, viewModel, UomEqual.Check));
            await viewModel.PageForwardAsync();
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage2, viewModel, UomEqual.Check));
            await viewModel.PageBackAsync();
            Assert.True(CollectionPageViewModelChecker.Check(expectedPage1, viewModel, UomEqual.Check));
        }
    }
}
