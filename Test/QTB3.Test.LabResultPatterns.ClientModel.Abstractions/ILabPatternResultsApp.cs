using System.Threading.Tasks;

namespace QTB3.Test.LabResultPatterns.ClientModel.Abstractions
{
    public interface ILabPatternResultsApp
    {
        // exogenous state change
        Task LoginManagerShouldRequestRelogin(); // to simulate token expiration

        // start
        Task LaunchApplication();

        // Login page
        Task ClickLoginOnLoginPage();

        // Main page
        Task ClickUomsOnMainPage();

        // Uoms collection page
        Task ClickBackOnUomsPage();
        Task ClickPageForwardOnUomsPage();
        Task ClickPageBackOnUomsPage();
        Task SearchOnUomsPage(string searchText);
        Task ClickDeleteOnUomsPageToolbar();
        Task SelectUomItemToDelete(int index);
        Task ClickEditOnUomsPageToolbar();
        Task SelectUomItemToEdit(int index);
        Task ClickAddOnUomsPage();

        // Uom item page
        Task EnterNameOnUomItemPage(string name);
        Task EnterDescriptionOnUomItemPage(string name);
        Task ClickSaveOnUomItemPage();
        
    }
}