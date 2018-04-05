using System;
using System.Threading.Tasks;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Client.LabResultPatterns.Common.Controllers
{
    public class MainPageController : IMainPageController
    {
        private readonly IMainPage _mainPage;
        private readonly IMainPageViewModel _mainPageViewModel;
        private readonly ILrpNavigation _navigation;
        private readonly ICollectionMvcBuilder<Uom> _uomMvcBuilder;
 
        public MainPageController
        (
            IMainPage mainPage,
            IMainPageViewModel mainPageViewModel,
            ILrpNavigation navigation,
            ICollectionMvcBuilder<Uom> uomMvcBuilder
        )
        {
            _mainPage = mainPage ?? throw new ArgumentNullException(nameof(mainPage));
            _mainPageViewModel = mainPageViewModel ?? throw new ArgumentNullException(nameof(mainPageViewModel));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _uomMvcBuilder = uomMvcBuilder ?? throw new ArgumentNullException(nameof(uomMvcBuilder));
            WireController();
        }

        private void WireController()
        {
            _mainPage.OnAppearingCalledEvent += MainPageOnAppearingCalled;
            _mainPage.UomWorkflowStartedEvent += OnUomWorkflowClicked;
        }

        private async void MainPageOnAppearingCalled(IMainPageOnAppearingEventArgs args)
        {
            if (!args.Handled)
            {
                await _mainPageViewModel.RefreshLinks();
                args.Tcs.SetResult(null);
            }
        }

        private async void OnUomWorkflowClicked(TaskCompletionSource<object> tcs)
        {
            await _uomMvcBuilder.BuildAsync(_navigation, LrpConstants.UomPageTitle);
            tcs.SetResult(null);
        }
    }
}
