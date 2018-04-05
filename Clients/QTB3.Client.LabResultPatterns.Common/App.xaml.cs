using Autofac;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.DI;
using QTB3.Client.LabResultPatterns.Common.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace QTB3.Client.LabResultPatterns.Common
{
    public partial class App
    {

        private readonly IJwtTokenManager _tokenManager;
        private readonly IMainPageController _mainPageController;
        private readonly ILoginPageController _loginPageController;

        // called by UWP apps
        public void UpdateRedirectUri(string url)
        {
            _tokenManager.UpdateRedirectUri(url);
        }

        public App()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<Services2Module>();
            builder.RegisterModule<LrpFactorsModule>();
            var container = builder.Build();

            // all of the following components are Autofac app-lifetime scope

            _tokenManager = container.Resolve<IJwtTokenManager>();

            var mp = container.Resolve<IMainPage>();
            MainPage = new NavigationPage(mp as ContentPage);

            var lrpNavigation = new LrpNavigation(MainPage.Navigation);

            // By passing the mainPage to the LoginPageController
            // before passing it to the MainPageController,
            // we ensure that the subscriptions to MainPage's
            // OnAppearingCalledEvent gives priority to the LoginPageController;
            _loginPageController = container.Resolve<ILoginPageController>
            (
                new NamedParameter("navigation", lrpNavigation),
                new NamedParameter("mainPage", mp)
            );

            var mainPageViewModel = container.Resolve<IMainPageViewModel>
            (
                new NamedParameter("pageTitle", LrpConstants.MainPageTitle)
            );
            mp.BindingContext = mainPageViewModel;
            _mainPageController = container.Resolve<IMainPageController>
            (
                new NamedParameter("mainPage", mp),
                new NamedParameter("mainPageViewModel", mainPageViewModel),
                new NamedParameter("navigation", lrpNavigation)
            );



        }
    }
}