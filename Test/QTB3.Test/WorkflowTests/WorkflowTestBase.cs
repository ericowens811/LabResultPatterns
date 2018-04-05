using Microsoft.Data.Sqlite;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Test.LabResultPatterns.ClientModel;
using QTB3.Test.LabResultPatterns.Support.Checkers.TopLevelChecking;
using QTB3.Test.LabResultPatterns.Support.Db;
using QTB3.Test.LabResultPatterns.Support.TestDevice;
using QTB3.Test.LabResultPatterns.Support.TestUser;

namespace QTB3.Test.WorkflowTests
{
    public class WorkflowTestBase: SqlitePropertyContextTestBase
    {
        public LabResultPatternsTestUser GetTestUser
        (
            SqliteConnection deviceConnection,
            SqliteConnection modelConnection
        )
        {
            // Funcs<DbContext> for appModel and pageCheckManager
            PropertyContext GetDeviceContext()
            {
                return GetPropertyContext(deviceConnection);
            }

            PropertyContext GetModelContext()
            {
                return GetPropertyContext(modelConnection);
            }

            // create the AppModel
            var appModel = new LabPatternResultsAppModel(GetModelContext);

            // create the TestDevice
            var testDevice = new LabPatternResultsTestDevice();

            // create the PageChecker
            var pageCheckManager = new PageCheckManager(GetDeviceContext, GetModelContext);

            return new LabResultPatternsTestUser(appModel, testDevice, pageCheckManager);
        }
    }
}
