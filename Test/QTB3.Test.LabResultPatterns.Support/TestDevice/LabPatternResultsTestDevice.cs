using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.DI;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.ClientModel.Abstractions;
using QTB3.Test.LabResultPatterns.Support.DI;
using QTB3.Test.LabResultPatterns.Support.Jwt;
using QTB3.Test.LabResultPatterns.Support.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace QTB3.Test.LabResultPatterns.Support.TestDevice
{

    public class LabPatternResultsTestDevice : ILabPatternResultsApp
    {
        private TestJwtTokenManager _jwtTokenManager;
        private bool _reloginRequested;

        private ILoginPageController _loginPageController;
        private IMainPageController _mainPageController;

        public ILrpNavigation Navigation { get; private set; }
        private MainPage _mainPage;

        public LabPatternResultsTestDevice()
        {
            // MockForms -- Client.Common will behave (mostly) as if on a device
            // see note below about view life cycle OnAppearing events
            MockForms.Init();
        }

        public async Task LaunchApplication()
        {
            // The App class does this work in the actual client
            // including initializing the singleton-lifestyle LoginWorkflowController with Navigation
            // (see QTB3.Client.LabResultPatterns.Common/App.xaml.cs)

            // We overload the DI to
            // -- set up a test HttpClient to be used by the client application service
            // -- set up in-memory test services that runs the read and write services
            // -- set up a TestLrpNavigator that runs the page lifecycle OnAppearing calls
            //
            // Note that the ASP.NET Startup used for in this testbench alters  
            // -- the rest basepath, 
            // -- authorization 
            //        -- use local JWT token generation rather than Azure AD B2C
            //        -- for now, bypass gathering credentials and just issue token
            // -- uses the in-memory Sqlite dB

            var builder = new ContainerBuilder();
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<Services2Module>();
            builder.RegisterModule<TestFactorsModule>();
            var container = builder.Build();

            // the test token manager can be induced to ask for relogin as if the token has expired
            _jwtTokenManager = container.Resolve<IJwtTokenManager>() as TestJwtTokenManager;

            // get the MainPage from the container 
            // and wrap it in a NavigationPage, just as the App does...
            _mainPage = container.Resolve<IMainPage>() as MainPage;
            var navMainPage = new NavigationPage(_mainPage);

            // assign the navigation proxy
            Navigation = new TestLrpNavigation(_mainPage.Navigation);

            _loginPageController = container.Resolve<ILoginPageController>
            (
                new NamedParameter("navigation", Navigation),
                new NamedParameter("mainPage", _mainPage)
            );

            var mainPageViewModel = container.Resolve<IMainPageViewModel>
            (
                new NamedParameter("pageTitle", LrpConstants.MainPageTitle)
            );
            _mainPage.BindingContext = mainPageViewModel;
            _mainPageController = container.Resolve<IMainPageController>
            (
                new NamedParameter("mainPage", _mainPage),
                new NamedParameter("mainPageViewModel", mainPageViewModel),
                new NamedParameter("navigation", Navigation)
            );

            // launch the main page
            await _mainPage.OnAppearingAsync();
        }

        private ILoginPage GetLoginPage()
        {
            var loginPage = Navigation.ModalStack.Last() as ILoginPage;
            Assert.NotNull(loginPage);
            return loginPage;
        }

        private IMainPage GetMainPage()
        {
            // verify MainPage is visible
            Assert.AreEqual(0, Navigation.ModalStack.Count);
            var mainPage = Navigation.NavigationStack.Last() as IMainPage;
            Assert.NotNull(mainPage);
            return mainPage;
        }

        private ICollectionPage<Uom> GetUomCollectionPage()
        {
            // verify UomCollectionPage is visible
            Assert.AreEqual(0, Navigation.ModalStack.Count);
            var uomsPage = Navigation.NavigationStack.Last() as ICollectionPage<Uom>;
            Assert.NotNull(uomsPage);
            return uomsPage;
        }

        private UomItemPage GetUomItemPage()
        {
            // verify UomItemPage is visible
            Assert.AreEqual(0, Navigation.ModalStack.Count);
            var uomItemPage = Navigation.NavigationStack.Last() as UomItemPage;
            Assert.NotNull(uomItemPage);
            return uomItemPage;
        }

        public Task LoginManagerShouldRequestRelogin()
        {
            _jwtTokenManager.SetNextGetTokenToRequestRelogin();
            _reloginRequested = true;
            return Task.CompletedTask;
        }

        public async Task ClickLoginOnLoginPage()
        {
            await GetLoginPage().OnLoginClickedAsync();
        }

        public async Task ClickUomsOnMainPage()
        {
            await GetMainPage().OnUomsClickedAsync();
        }

        public async Task ClickBackOnUomsPage()
        {
            await Navigation.PopAsync();
        }

        public async Task ClickPageForwardOnUomsPage()
        {
            await GetUomCollectionPage().PageForwardClickedAsync();
        }

        public async Task ClickPageBackOnUomsPage()
        {
            await GetUomCollectionPage().PageBackClickedAsync();
        }

        public async Task SearchOnUomsPage(string searchText)
        {
            var uomsPage = GetUomCollectionPage();
            uomsPage.SearchBar.Text = searchText;
            await uomsPage.SearchRequestedAsync();
        }

        public async Task ClickDeleteOnUomsPageToolbar()
        {
            await GetUomCollectionPage().DeleteButtonClickedAsync();
        }

        public async Task SelectUomItemToDelete(int index)
        {
            var uomsPage = GetUomCollectionPage();
            await uomsPage.OnItemSelectedAsync(uomsPage.ListViewSourceProbe.ToList()[index]);
        }

        public async Task ClickEditOnUomsPageToolbar()
        {
            await GetUomCollectionPage().EditButtonClickedAsync();
        }

        public async Task ClickAddOnUomsPage()
        {
            await GetUomCollectionPage().AddButtonClickedAsync();
        }

        public async Task SelectUomItemToEdit(int index)
        {
            var uomsPage = GetUomCollectionPage();
            await uomsPage.OnItemSelectedAsync(uomsPage.ListViewSourceProbe.ToList()[index]);
        }

        public async Task EnterNameOnUomItemPage(string name)
        {
            GetUomItemPage().NameEntry.Text = name;
        }

        public async Task EnterDescriptionOnUomItemPage(string name)
        {
            GetUomItemPage().DescriptionEntry.Text = name;
        }

        public async Task ClickSaveOnUomItemPage()
        {
            await GetUomItemPage().SaveToolbarItemClickedAsync();
        }
    }
}
