using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.Db;

namespace QTB3.Test.WorkflowTests
{
    [TestFixture]
    public class UomWorkflowTests : WorkflowTestBase
    {
        [Test]
        [Category("Workflow")]
        public async Task AddUom()
        {
            var deviceConnection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            var modelConnection = new SqliteConnection("DataSource=modeldb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(deviceConnection, GetPropertyContext, UomTestData.GetInitialData());
                new DbInitializer().Initialize(modelConnection, GetPropertyContext, UomTestData.GetInitialData());
                var testUser = GetTestUser(deviceConnection, modelConnection);

                //
                // THE TEST
                //
                await testUser.LaunchApplication();
                await testUser.ClickLoginOnLoginPage();
                await testUser.ClickUomsOnMainPage();
                await testUser.ClickAddOnUomsPage();
                await testUser.EnterNameOnUomItemPage("zzz");
                await testUser.EnterDescriptionOnUomItemPage("Sleep");
                await testUser.ClickSaveOnUomItemPage();
                await testUser.ClickBackOnUomsPage();
            }
            finally
            {
                deviceConnection.Close();
                modelConnection.Close();
            }
        }

        [Test]
        [Category("Workflow")]
        public async Task ThrashUoms()
        {
            var deviceConnection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            var modelConnection = new SqliteConnection("DataSource=modeldb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(deviceConnection, GetPropertyContext, UomTestData.GetInitialData());
                new DbInitializer().Initialize(modelConnection, GetPropertyContext, UomTestData.GetInitialData());
                var testUser = GetTestUser(deviceConnection, modelConnection);
                //
                // THE TEST
                //
                await testUser.LaunchApplication();
                await testUser.ClickLoginOnLoginPage();
                await testUser.ClickUomsOnMainPage();
                await testUser.ClickBackOnUomsPage();
                await testUser.ClickUomsOnMainPage();
                await testUser.ClickPageForwardOnUomsPage();
                await testUser.ClickPageForwardOnUomsPage();
                await testUser.ClickPageForwardOnUomsPage();
                await testUser.ClickPageBackOnUomsPage();
                await testUser.ClickPageBackOnUomsPage();
                await testUser.ClickBackOnUomsPage();

                await testUser.ClickUomsOnMainPage();
                await testUser.SearchOnUomsPage("j");
                await testUser.SearchOnUomsPage("");
                await testUser.ClickBackOnUomsPage();
                await testUser.ClickUomsOnMainPage();
                await testUser.SearchOnUomsPage("i");
                await testUser.ClickBackOnUomsPage();
                await testUser.ClickUomsOnMainPage();
                await testUser.ClickDeleteOnUomsPageToolbar();
                await testUser.SelectUomItemToDelete(5);
                await testUser.ClickBackOnUomsPage();

                await testUser.ClickUomsOnMainPage();
                await testUser.ClickDeleteOnUomsPageToolbar();
                await testUser.ClickEditOnUomsPageToolbar();
                await testUser.SelectUomItemToEdit(2);
                await testUser.EnterNameOnUomItemPage("UpdatedName");
                await testUser.EnterDescriptionOnUomItemPage("UpdatedDescription");
                await testUser.ClickSaveOnUomItemPage();
                await testUser.ClickBackOnUomsPage();

                await testUser.ClickUomsOnMainPage();
                await testUser.SearchOnUomsPage("Updated");
                await testUser.SelectUomItemToEdit(0);
                await testUser.EnterNameOnUomItemPage("UpdatedAgain");
                await testUser.ClickSaveOnUomItemPage();
                await testUser.ClickBackOnUomsPage();
            }
            finally
            {
                deviceConnection.Close();
                modelConnection.Close();
            }
        }
    }
}
