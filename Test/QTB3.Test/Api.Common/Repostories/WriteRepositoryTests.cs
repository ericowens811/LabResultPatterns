using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.Db;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.ConstructorTesting;
using QTB3.Test.Support.Db;

namespace QTB3.Test.Api.Common.Repostories
{
    // These tests rely on the data found in QTB3.Test.LabResultPatterns.Support.TestData
    // Keep it handy when debugging...
    [TestFixture]
    public class WriteRepositoryTests: SqlitePropertyContextTestBase
    {
        [Test]
        [Category("WriteRepository")]
        public void Constructor()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                using (var context = GetPropertyContext(connection))
                {
                    ConstructorTests<UomWriteRepository>
                        .For(typeof(PropertyContext))
                        .Fail(new object[] { null }, typeof(ArgumentNullException), "Null context")
                        .Succeed(new object[] { context }, "Constructor arguments valid")
                        .Assert();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public void PutNullItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    Assert.ThrowsAsync<ArgumentNullException>
                    (
                        async () => await uomRepository.PutAsync(null)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public async Task PutItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    var itemToPut = new Uom ( 3, "cubits", "The description");
                    await uomRepository.PutAsync(itemToPut);

                    var uoms = await context.Uoms.OrderBy(u => u.Name).ToListAsync();
                    Assert.NotNull(uoms);
                    Assert.AreEqual(UomTestData.GetUomsArray().Length, uoms.Count);
                    Assert.True(UomEqual.Check(itemToPut, uoms[0]));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public void PostNullItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    Assert.ThrowsAsync<ArgumentNullException>
                    (
                        async () => await uomRepository.PostAsync(null)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public async Task PostDuplicate()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    var newUom = new Uom(0, "mg/dL", "Errant duplicate");
                    var exception = Assert.ThrowsAsync<DbUpdateException>(async () => await uomRepository.PostAsync(newUom));
                    var message = exception.InnerException.Message;
                    Assert.True(message.Contains("SQLite Error 19"));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public async Task PostItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    var newUom = new Uom(0, "Cubits", "The description");
                    var postedItem = await uomRepository.PostAsync(newUom);
                    Assert.NotNull(postedItem);
                    Assert.True(UomEqual.Check(newUom, postedItem));

                    var uoms = await context.Uoms.ToListAsync();
                    Assert.NotNull(uoms);
                    Assert.AreEqual(UomTestData.GetUomsArray().Length+1, uoms.Count);

                    var resultUoms = await context.Uoms.Where(u => u.Name == "Cubits").ToListAsync();
                    Assert.NotNull(resultUoms);
                    Assert.AreEqual(1, resultUoms.Count);
                    Assert.True(UomEqual.Check(postedItem, resultUoms[0]));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public void DeleteItemId0()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    Assert.ThrowsAsync<ArgumentOutOfRangeException>
                    (
                        async () => await uomRepository.DeleteAsync(0)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("WriteRepository")]
        public async Task DeleteItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomWriteRepository(context);
                    await uomRepository.DeleteAsync(2);

                    var uoms = await context.Uoms.Where(u=>u.Name == "in").ToListAsync();
                    Assert.AreEqual(0, uoms.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}

