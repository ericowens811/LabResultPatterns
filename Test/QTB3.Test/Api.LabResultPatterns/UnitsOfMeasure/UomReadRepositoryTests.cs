using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.Db;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.PageMakers;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.ConstructorTesting;
using QTB3.Test.Support.Db;

namespace QTB3.Test.Api.LabResultPatterns.UnitsOfMeasure
{
    [TestFixture]
    public class UomReadRepositoryTests : SqlitePropertyContextTestBase
    {
        [Test]
        [Category("UomReadRepository")]
        public void ReadRepositoryConstructorThrows()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                using (var context = GetPropertyContext(connection))
                {
                    ConstructorTests<UomReadRepository>
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
        [Category("UomReadRepository")]
        public void GetSingleUomId0ThrowsArgumentOutOfRangeException()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    Assert.ThrowsAsync<ArgumentOutOfRangeException>
                    (
                        async () => await uomRepository.GetAsync(0)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetSingleUom()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    var uom = await uomRepository.GetAsync(1);
                    Assert.NotNull(uom);
                    Assert.True(UomEqual.Check(testData[0], uom));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetSingleUomNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    var uom = await uomRepository.GetAsync(111111);
                    Assert.Null(uom);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetUomCollectionSkipLessThan0()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    Assert.ThrowsAsync<ArgumentOutOfRangeException>
                    (
                        async () => await uomRepository.GetAsync("", -1, 20)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetUomCollectionTakeLessThan1()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    Assert.ThrowsAsync<ArgumentOutOfRangeException>
                    (
                        async () => await uomRepository.GetAsync("", 0, 0)
                    );
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetFilteredUomCollection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    var expectedPage = PageMaker.GetExpectedPage(testData, "j", 0, 20);
                    var actualPage = await uomRepository.GetAsync("j", 0, 20) as Page<Uom>;
                    Assert.True(PageEqual.Check(expectedPage, actualPage, UomEqual.Check));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetUnfilteredUomCollection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    var expectedPage = PageMaker.GetExpectedPage(testData, "", 0, 20);
                    var actualPage = await uomRepository.GetAsync("", 0, 20) as Page<Uom>;
                    Assert.NotNull(actualPage);
                    Assert.True(PageEqual.Check(expectedPage, actualPage, UomEqual.Check));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadRepository")]
        public async Task GetPageNotFound()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                using (var context = GetPropertyContext(connection))
                {
                    var uomRepository = new UomReadRepository(context);
                    var expectedPage = PageMaker.GetExpectedPage<Uom>(testData, "", 1000, 20);
                    var actualPage = await uomRepository.GetAsync("", 1000, 20) as Page<Uom>;
                    Assert.True(PageEqual.Check(expectedPage, actualPage, UomEqual.Check));
                }
            }
            finally
            {
                connection.Close();
            }
        }


    }
}
