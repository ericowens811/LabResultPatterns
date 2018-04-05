using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QTB3.Model.LabResultPatterns.Contexts;


namespace QTB3.Test.LabResultPatterns.Support.Db
{
    public class SqlitePropertyContextTestBase
    {
        public PropertyContext GetPropertyContext(SqliteConnection connection)
        {
            connection.Open();
            var options = new DbContextOptionsBuilder<PropertyContext>()
                .UseSqlite(connection)
                .Options;
            return new PropertyContext(options);
        }
    }
}
