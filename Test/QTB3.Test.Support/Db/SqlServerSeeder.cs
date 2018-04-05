using System;
using NUnit.Framework;

//using QTB3.LabResultPatterns.Model.Contexts;
//using QTB3.Test.TestSupport.Db;
//using QTB3.Test.TestSupport.TestData;

namespace QTB3.Test.Support.Db
{
    [TestFixture]
    public class SqlServerSeeder
    {
        /*
        see https://stackoverflow.com/questions/36401991/using-localdb-with-service-fabric
        to use localdb with service fabric:
        sqllocaldb i
        sqllocaldb stop mssqllocaldb
        sqllocaldb delete mssqllocaldb
        sqllocaldb create mssqllocaldb
        sqllocaldb start mssqllocaldb
        sqllocaldb share mssqllocaldb sharedlocal
        sqllocaldb stop MSSQLLocalDB
        sqllocaldb start MSSQLLocalDB
        > add the database named in the next command, then run:
        "c:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE" -S "(localdb)\.\sharedlocal" -d "LabPatternDb" -Q"create login [nt authority\network service] FROM windows with DEFAULT_DATABASE=LabPatternDb;use LabPatternDb;exec sp_addrolemember 'db_owner', 'nt authority\network service';"
        
        to connect mssql mananagement studio to localdb
        http://nikgupta.net/2015/12/10/connect-localdb-from-management-studio/
        sqllocaldb start mssqllocaldb // THIS IS VERY IMPORTANT
        sqllocaldb i mssqllocaldb
            - returns Instance Pipe Name - this is the server name in management studio
            for instance: np:\\.\pipe\LOCALDB#SH002DDA\tsql\query
         */
        //Server=tcp:cogmod.database.windows.net,1433;Initial Catalog=LabResultPatterns;Persist Security Info=False;User ID=eric;Password=Lewisacid1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;


        //[Test]
        [Category("LocalDbSeed")]
        //[Ignore("Use only for initialization of dB")]
        public void TestSeed()
        {
            try
            {
                //PropertyContext GetContext()
                //{
                //    var options = new DbContextOptionsBuilder<PropertyContext>().UseSqlServer(
                //        //"Server=(localdb)\\.\\sharedlocal;Database=LabPatternDb;Integrated Security=SSPI;Trusted_Connection=True;MultipleActiveResultSets=true"
                //        "Server=tcp:cogmod.database.windows.net,1433;Initial Catalog=LabResultPatterns;Persist Security Info=False;User ID=eric;Password=Lewisacid1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                //        )
                //        .Options;
                //    return new PropertyContext(options);
                //}

                //new DbInitializer().Initialize(GetContext, UomTestData.GetInitialDataNoIds());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
