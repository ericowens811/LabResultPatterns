using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.ClientModel.Abstractions;
using QTB3.Test.LabResultPatterns.ClientModel.AppState;
using QTB3.Test.LabResultPatterns.ClientModel.CollectionPage;
using QTB3.Test.LabResultPatterns.ClientModel.LoginComponents;
using QTB3.Test.LabResultPatterns.ClientModel.MainPageComponents;
using QTB3.Test.LabResultPatterns.ClientModel.UomComponents;

namespace QTB3.Test.LabResultPatterns.ClientModel
{
    public class LabPatternResultsAppModel : ILabPatternResultsApp
    {
        private Func<DbContext> _getModelContext;

        private bool _reloginRequested;

        public int NavCount { get; private set; }
        public int ModalCount { get; private set; }
        public NavState NavState { get; private set; }
        public ModalState ModalState { get; private set; }

        public LoginPageModel ExpectedLoginPage { get; }
        public MainPageModel ExpectedMainPage { get; }
        public CollectionPageModel<Uom> ExpectedUomsCollectionPage { get; set; }
        public UomItemPageModel ExpectedUomItemPage { get; set; }

        private IReadRepository<Uom> GetUomReadRepository()
        {
            return new UomReadRepository(_getModelContext() as PropertyContext);
        }

        private IWriteRepository<Uom> GetUomWriteRepository()
        {
            return new UomWriteRepository(_getModelContext() as PropertyContext);
        }

        private CollectionPageModel<Uom> GetNewExpectedUomsPage()
        {
            return new CollectionPageModel<Uom>(GetUomReadRepository, GetUomWriteRepository) { TitleText = "Uoms" };
        }

        private UomItemPageModel GetNewExpectedUomItemPage()
        {
            return new UomItemPageModel(GetUomReadRepository, GetUomWriteRepository);
        }

        public LabPatternResultsAppModel(Func<DbContext> getModelContext)
        {
            _getModelContext = getModelContext;

            ExpectedLoginPage = new LoginPageModel("Login");
            ExpectedMainPage = new MainPageModel("Home");

            NavCount = 0;
            ModalCount = 0;
            
            NavState = NavState.Empty;
            ModalState = ModalState.Empty;
        }

        public Task LoginManagerShouldRequestRelogin()
        {
            _reloginRequested = true;
            return Task.CompletedTask;
        }

        private bool CheckForRelogin()
        {
            if (!_reloginRequested) return false;
            _reloginRequested = false;
            ModalState = ModalState.Login;
            ExpectedLoginPage.OnAppearing();
            ModalCount++;
            return true;
        }

        public Task LaunchApplication()
        {
            // legitimize action
            if (NavState != NavState.Empty && ModalState != ModalState.Empty)
            {
                throw new Exception("Can't LaunchApplication if not Unlaunched");
            }

            NavState = NavState.OnMainPage;
            NavCount++;
            ModalState = ModalState.Login;
            ExpectedLoginPage.OnAppearing();
            ModalCount++;
            return Task.CompletedTask;
        }

        public async Task ClickLoginOnLoginPage()
        {
            // legitimize action
            if (ModalState != ModalState.Login)
                throw new Exception("Can't ClickLogin if not on Login page");

            ModalState = ModalState.Empty;
            ModalCount--;
            switch (NavState)
            {
                case NavState.OnMainPage:
                    await ExpectedMainPage.OnAppearingAsync();
                    break;
                case NavState.OnUomsPage:
                    await ExpectedUomsCollectionPage.OnReappearingAsync();
                    break;
                case NavState.OnUomItemPage:
                    await ExpectedUomItemPage.OnAppearingAsync();
                    break;
                case NavState.Empty:
                    break;
                default:
                    break;
            }
        }

        public async Task ClickUomsOnMainPage()
        {
            // legitimize action
            if (NavState != NavState.OnMainPage)
                throw new Exception("Can't ClickUomsOnMainPage() if not on Main page");

            ExpectedUomsCollectionPage = GetNewExpectedUomsPage();
            NavState = NavState.OnUomsPage;
            NavCount++;
            ExpectedUomsCollectionPage.ClickEditOnToolbar();
            if (!CheckForRelogin())
            {
                await ExpectedUomsCollectionPage.OnAppearingAsync();
            }
        }

        public async Task ClickBackOnUomsPage()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't ClickBackOnUomsPage() if not on Uoms page");

            NavState = NavState.OnMainPage;
            await ExpectedMainPage.OnAppearingAsync();
            NavCount--;
        }

        public async Task ClickPageForwardOnUomsPage()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't ClickNextPageOnUomsPage() if not on Uoms page");

            if (!CheckForRelogin())
            { 
                await ExpectedUomsCollectionPage.PageForwardTappedAsync();
            }
        }

        public async Task ClickPageBackOnUomsPage()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't ClickNextPageOnUomsPage() if not on Uoms page");

            if (!CheckForRelogin())
            {
                await ExpectedUomsCollectionPage.PageBackTappedAsync();
            }
        }

        public async Task SearchOnUomsPage(string searchText)
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't SearchOnUomsPage() if not on Uoms page");

            ExpectedUomsCollectionPage.SetSearchText(searchText);
            if (!CheckForRelogin())
            {
                await ExpectedUomsCollectionPage.Search();
            }
        }

        public async Task ClickDeleteOnUomsPageToolbar()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't ClickDeleteOnUomsPageToolbar() if not on Uoms page");

            ExpectedUomsCollectionPage.ClickDeleteOnToolbar();
        }

        public async Task SelectUomItemToDelete(int index)
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't SelectUomItemToDelete() if not on Uoms page"); ;

            if (!CheckForRelogin())
            {
                await ExpectedUomsCollectionPage.DeleteAsync(index);
            }
        }

        public async Task ClickAddOnUomsPage()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't SelectUomItemToEdit() if not on Uoms page");

            ExpectedUomItemPage = GetNewExpectedUomItemPage();
            ExpectedUomItemPage.SetUomId(0);
            ExpectedUomItemPage.SetItemIsBeingEdited(false);
            NavState = NavState.OnUomItemPage;
            NavCount++;
            if (!CheckForRelogin())
            {
                await ExpectedUomItemPage.OnAppearingAsync();
            }
        }

        public async Task ClickEditOnUomsPageToolbar()
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't ClickEditOnUomsPageToolbar() if not on Uoms page");

            ExpectedUomsCollectionPage.ClickEditOnToolbar();
        }

        public async Task SelectUomItemToEdit(int index)
        {
            // legitimize action
            if (NavState != NavState.OnUomsPage)
                throw new Exception("Can't SelectUomItemToEdit() if not on Uoms page");

            var selectedListItemId = ExpectedUomsCollectionPage.Select(index).Id;
            ExpectedUomItemPage = GetNewExpectedUomItemPage();
            ExpectedUomItemPage.SetUomId(selectedListItemId);
            ExpectedUomItemPage.SetItemIsBeingEdited(true);
            NavState = NavState.OnUomItemPage;
            NavCount++;
            if (!CheckForRelogin())
            {
                await ExpectedUomItemPage.OnAppearingAsync();
            }
        }

        public async Task EnterNameOnUomItemPage(string name)
        {
            // legitimize action
            if (NavState != NavState.OnUomItemPage)
                throw new Exception("Can't EnterNameOnUomItemPage() if not on Uom Item page");

            ExpectedUomItemPage.EnterName(name);
        }

        public async Task EnterDescriptionOnUomItemPage(string description)
        {
            // legitimize action
            if (NavState != NavState.OnUomItemPage)
                throw new Exception("Can't EnterDescriptionOnUomItemPage() if not on Uom Item page");

            ExpectedUomItemPage.EnterDescription(description);
        }

        public async Task ClickSaveOnUomItemPage()
        {
            // legitimize action
            if (NavState != NavState.OnUomItemPage)
                throw new Exception("Can't ClickSaveOnUomItemPage() if not on Uom Item page");

            await ExpectedUomItemPage.Save();
            NavState = NavState.OnUomsPage;
            NavCount--;
            if (!CheckForRelogin())
            {
                await ExpectedUomsCollectionPage.OnReappearingAsync();
            }
        }
    }
}
