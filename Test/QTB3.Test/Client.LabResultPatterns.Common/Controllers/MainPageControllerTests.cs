using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.Controllers;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using Xamarin.Forms.Mocks;

namespace QTB3.Test.Client.LabResultPatterns.Common.Controllers
{
    public class MainPageControllerMocks
    {
        public Mock<IMainPageViewModel> MainPageViewModel { get; set; }
        public Mock<ILrpNavigation> LrpNavigation { get; set; }
        public Mock<ICollectionMvcBuilder<Uom>> UomMvcBuilder { get; set; }

        public MainPageControllerMocks()
        {
            MainPageViewModel = new Mock<IMainPageViewModel>(MockBehavior.Strict);
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
            UomMvcBuilder = new Mock<ICollectionMvcBuilder<Uom>>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class MainPageControllerTests
    {
        [Test]
        [Category("MainPageController")]
        public void Constructor()
        {
            var mainPage = new Mock<IMainPage>().Object;
            var viewModel = new Mock<IMainPageViewModel>().Object;
            var lrpNavigation = new Mock<ILrpNavigation>().Object;
            var uomMvcBuilder = new Mock<ICollectionMvcBuilder<Uom>>().Object;

            ConstructorTests<MainPageController>
                .For
                (
                    typeof(IMainPage),
                    typeof(IMainPageViewModel),
                    typeof(ILrpNavigation),
                    typeof(ICollectionMvcBuilder<Uom>)
                )
                .Fail(new object[] { null, viewModel, lrpNavigation, uomMvcBuilder }, typeof(ArgumentNullException), "Null mainPage.")
                .Fail(new object[] { mainPage, null, lrpNavigation, uomMvcBuilder }, typeof(ArgumentNullException), "Null viewModel.")
                .Fail(new object[] { mainPage, viewModel, null, uomMvcBuilder }, typeof(ArgumentNullException), "Null lrpNavigation.")
                .Fail(new object[] { mainPage, viewModel, lrpNavigation, null }, typeof(ArgumentNullException), "Null uomMvcBuilder.")
                .Succeed(new object[] { mainPage, viewModel, lrpNavigation, uomMvcBuilder }, "Constructor args valid")
                .Assert();
        }

        public MainPageController BuildController(MainPageControllerMocks mocks, IMainPage mainPage)
        {
            return new MainPageController
            (
                mainPage,
                mocks.MainPageViewModel.Object,
                mocks.LrpNavigation.Object,
                mocks.UomMvcBuilder.Object
            );
        }

        [Test]
        [Category("MainPageController")]
        public async Task OnAppearing_ArgsHandled()
        {
            MockForms.Init();
            var mainPage = new MainPage();
            mainPage.OnAppearingCalledEvent += (args) =>
            {
                args.Handled = true;
                args.Tcs.SetResult(null);
            };
            var mocks = new MainPageControllerMocks();
            BuildController(mocks, mainPage);

            await mainPage.OnAppearingAsync();
        }

        [Test]
        [Category("MainPageController")]
        public async Task OnAppearing_ArgsNotHandled()
        {
            MockForms.Init();
            var mainPage = new MainPage();
            mainPage.OnAppearingCalledEvent += (args) => { };
            var mocks = new MainPageControllerMocks();
            mocks.MainPageViewModel.Setup(v => v.RefreshLinks()).Returns(Task.CompletedTask);
            BuildController(mocks, mainPage);

            await mainPage.OnAppearingAsync();
        }

        [Test]
        [Category("MainPageController")]
        public async Task UomWorkflowClicked()
        {
            MockForms.Init();
            var mainPage = new MainPage();
            var mocks = new MainPageControllerMocks();
            var uomPage = new Mock<ICollectionPage<Uom>>(MockBehavior.Strict);
            mocks.UomMvcBuilder
                .Setup(b => b.BuildAsync(mocks.LrpNavigation.Object, LrpConstants.UomPageTitle))
                .Returns(Task.CompletedTask);
            BuildController(mocks, mainPage);

            await mainPage.OnUomsClickedAsync();
        }


    }
}
