using System.Threading.Tasks;
using QTB3.Test.LabResultPatterns.ClientModel;
using QTB3.Test.LabResultPatterns.ClientModel.Abstractions;
using QTB3.Test.LabResultPatterns.Support.Checkers.TopLevelChecking;
using QTB3.Test.LabResultPatterns.Support.Db;
using QTB3.Test.LabResultPatterns.Support.TestDevice;

namespace QTB3.Test.LabResultPatterns.Support.TestUser
{
    public class LabResultPatternsTestUser : SqlitePropertyContextTestBase, ILabPatternResultsApp
    {
        private readonly LabPatternResultsAppModel _appModel;
        private readonly LabPatternResultsTestDevice _testDevice;
        private readonly PageCheckManager _pageCheckManager;

        public LabResultPatternsTestUser
        (
            LabPatternResultsAppModel appModel,
            LabPatternResultsTestDevice testDevice,
            PageCheckManager pageCheckManager
        )
        {
            _appModel = appModel;
            _testDevice = testDevice;
            _pageCheckManager = pageCheckManager;
        }

        //
        // Test actions
        //

        public async Task LoginManagerShouldRequestRelogin()
        {
            await _appModel.LoginManagerShouldRequestRelogin();
            await _testDevice.LoginManagerShouldRequestRelogin();
        }

        //
        // User actions
        //

        public async Task LaunchApplication()
        {
            // act on model
            await _appModel.LaunchApplication();

            // act on actual
            await _testDevice.LaunchApplication();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickLoginOnLoginPage()
        {
            // act on model
            await _appModel.ClickLoginOnLoginPage();

            // act on actual
            await _testDevice.ClickLoginOnLoginPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickUomsOnMainPage()
        {
            // act on model
            await _appModel.ClickUomsOnMainPage();

            // act on actual
            await _testDevice.ClickUomsOnMainPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickBackOnUomsPage()
        {
            // act on model
            await _appModel.ClickBackOnUomsPage();

            // act on actual
            await _testDevice.ClickBackOnUomsPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickPageForwardOnUomsPage()
        {
            // act on model
            await _appModel.ClickPageForwardOnUomsPage();

            // act on actual
            await _testDevice.ClickPageForwardOnUomsPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickPageBackOnUomsPage()
        {
            // act on model
            await _appModel.ClickPageBackOnUomsPage();

            // act on actual
            await _testDevice.ClickPageBackOnUomsPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task SearchOnUomsPage(string searchText)
        {
            // act on model
            await _appModel.SearchOnUomsPage(searchText);

            // act on actual
            await _testDevice.SearchOnUomsPage(searchText);

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickDeleteOnUomsPageToolbar()
        {
            // act on model
            await _appModel.ClickDeleteOnUomsPageToolbar();

            // act on actual
            await _testDevice.ClickDeleteOnUomsPageToolbar();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task SelectUomItemToDelete(int index)
        {
            // act on model
            await _appModel.SelectUomItemToDelete(index);

            // act on actual
            await _testDevice.SelectUomItemToDelete(index);

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickAddOnUomsPage()
        {
            // act on model
            await _appModel.ClickAddOnUomsPage();

            // act on actual
            await _testDevice.ClickAddOnUomsPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickEditOnUomsPageToolbar()
        {
            // act on model
            await _appModel.ClickEditOnUomsPageToolbar();

            // act on actual
            await _testDevice.ClickEditOnUomsPageToolbar();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task SelectUomItemToEdit(int index)
        {
            // act on model
            await _appModel.SelectUomItemToEdit(index);

            // act on actual
            await _testDevice.SelectUomItemToEdit(index);

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task EnterNameOnUomItemPage(string name)
        {
            // act on model
            await _appModel.EnterNameOnUomItemPage(name);

            // act on actual
            await _testDevice.EnterNameOnUomItemPage(name);

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task EnterDescriptionOnUomItemPage(string name)
        {
            // act on model
            await _appModel.EnterDescriptionOnUomItemPage(name);

            // act on actual
            await _testDevice.EnterDescriptionOnUomItemPage(name);

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }

        public async Task ClickSaveOnUomItemPage()
        {
            // act on model
            await _appModel.ClickSaveOnUomItemPage();

            // act on actual
            await _testDevice.ClickSaveOnUomItemPage();

            // verify page
            await _pageCheckManager.Check(_appModel, _testDevice);
        }



    }
}
