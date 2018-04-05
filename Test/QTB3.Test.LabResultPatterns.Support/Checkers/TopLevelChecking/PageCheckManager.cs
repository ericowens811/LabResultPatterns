using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Client.LabResultPatterns.Common.Views;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.ClientModel;
using QTB3.Test.LabResultPatterns.ClientModel.AppState;
using QTB3.Test.LabResultPatterns.Support.Checkers.CollectionPageChecking;
using QTB3.Test.LabResultPatterns.Support.Checkers.LoginComponents;
using QTB3.Test.LabResultPatterns.Support.Checkers.MainPageComponents;
using QTB3.Test.LabResultPatterns.Support.Checkers.UomComponents;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.TestDevice;


namespace QTB3.Test.LabResultPatterns.Support.Checkers.TopLevelChecking
{
    public class PageCheckManager
    {
        private readonly LoginPageChecker _loginPageChecker;
        private readonly MainPageChecker _mainPageChecker;
        private readonly CollectionPageChecker<Uom, UomViewCell> _uomsPageChecker;
        private readonly UomItemPageChecker _uomItemPageChecker;

        public PageCheckManager
        (
            Func<DbContext> getDeviceContext,
            Func<DbContext> getModelContext 
        )
        {
            _loginPageChecker = new LoginPageChecker();
            _mainPageChecker = new MainPageChecker();

            var uomViewCellChecker = new UomViewCellChecker();
            _uomsPageChecker = new CollectionPageChecker<Uom, UomViewCell>
                (uomViewCellChecker, getDeviceContext, getModelContext, UomEqual.CheckExceptId);
            _uomItemPageChecker = new UomItemPageChecker();           
        }

        public async Task Check
        (
            LabPatternResultsAppModel appModel,
            LabPatternResultsTestDevice testDevice 
        ) 
        {
            Assert.AreEqual(appModel.ModalCount, testDevice.Navigation.ModalStack.Count);
            Assert.AreEqual(appModel.NavCount, testDevice.Navigation.NavigationStack.Count);
            if (appModel.ModalCount > 0)
            {
                await CheckModalPage(appModel, testDevice);
            }
            else
            {
                await CheckNavPage(appModel, testDevice);
            }
        }

        private async Task CheckModalPage
        (
            LabPatternResultsAppModel appModel,
            LabPatternResultsTestDevice testDevice
        )
        {
            if (appModel.ModalState != ModalState.Login) return;
            var loginPage = testDevice.Navigation.ModalStack.Last() as LoginPage;
            Assert.NotNull(loginPage);
            _loginPageChecker.Check(appModel.ExpectedLoginPage, loginPage);
        }

        private async Task CheckNavPage
        (
            LabPatternResultsAppModel appModel,
            LabPatternResultsTestDevice testDevice
        )
        {
            switch (appModel.NavState)
            {
                case NavState.OnMainPage:
                    var mainPage = testDevice.Navigation.NavigationStack.Last() as MainPage;
                    Assert.NotNull(mainPage);
                    _mainPageChecker.Check(appModel.ExpectedMainPage, mainPage);
                    break;
                case NavState.OnUomsPage:
                    var uomsPage = testDevice.Navigation.NavigationStack.Last() as CollectionPage<Uom>;
                    Assert.NotNull(uomsPage);
                    await _uomsPageChecker.Check(appModel.ExpectedUomsCollectionPage, uomsPage);
                    break;
                case NavState.OnUomItemPage:
                    var uomItemPage = testDevice.Navigation.NavigationStack.Last() as UomItemPage;
                    Assert.NotNull(uomItemPage);
                    await _uomItemPageChecker.Check(appModel.ExpectedUomItemPage, uomItemPage);
                    break;
                case NavState.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
