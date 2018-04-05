using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.Db;

namespace QTB3.Test.Api.LabResultPatterns.ServiceIntegration
{
    [TestFixture]
    public class WriteServiceTests : ServiceTestBase
    {
        [Test]
        [Category("UomWriteService")]
        public async Task TryPutUomNoToken()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = new HttpRequestMessage(HttpMethod.Put, $"{EndpointBase}/lrp/uoms/2");
                httpMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(LrpSupportedMedia.LrpMediaTypeV2));
                var uomToPut = new Uom(2, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPut);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task TryPutUomGoodScopeBadUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Put, $"{EndpointBase}/lrp/uoms/2", GoodScopeBadUserClaimsList);
                var uomToPut = new Uom(2, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPut);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task TryPutUomBadScopeGoodUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Put, $"{EndpointBase}/lrp/uoms/2", BadScopeGoodUserClaimsList);
                var uomToPut = new Uom(2, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPut);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task TryPutUomBadScopeBadUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Put, $"{EndpointBase}/lrp/uoms/2", BadScopeBadUserClaimsList);
                var uomToPut = new Uom(2, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPut);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task PutUomGoodClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Put, $"{EndpointBase}/lrp/uoms/2", GoodClaimsList);
                var uomToPut = new Uom(2, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPut);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                using (var context = GetPropertyContext(connection))
                {
                    var uoms = await context.Uoms.Where(u => u.Id == 2).ToListAsync();
                    Assert.NotNull(uoms);
                    Assert.AreEqual(1, uoms.Count);
                    Assert.True(UomEqual.Check(uomToPut, uoms[0]));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task PostUomGoodClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Post, $"{EndpointBase}/lrp/uoms", GoodClaimsList);
                var uomToPost = new Uom(0, "NewName", "New Description");
                httpMessage.Content = GetWriteContent(uomToPost);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                using (var context = GetPropertyContext(connection))
                {
                    var uoms = await context.Uoms.Where(u => u.Name == "NewName").ToListAsync();
                    Assert.NotNull(uoms);
                    Assert.AreEqual(1, uoms.Count);
                    Assert.True(UomEqual.CheckExceptId(uomToPost, uoms[0]));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task PostInvalidUom()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Post, $"{EndpointBase}/lrp/uoms", GoodClaimsList);
                var uomToPost = new Uom
                (
                    id: 0,
                    name: "N_0123456789_0123456789_0123456789_0123456789_0123456789",
                    description: "Some Description"
                );
                httpMessage.Content = GetWriteContent(uomToPost);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var errorDictionary = JsonConvert.DeserializeObject<Dictionary<string, ReadOnlyCollection<string>>>(jsonString);
                Assert.AreEqual(1, errorDictionary.Count);
                errorDictionary.TryGetValue("Name", out var value);
                Assert.NotNull(value);
                Assert.AreEqual(1, value.Count);
                Assert.AreEqual("The field Name must be a string with a minimum length of 1 and a maximum length of 30.", value[0]);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task PostInvalidTwiceUom()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Post, $"{EndpointBase}/lrp/uoms", GoodClaimsList);
                var uomToPost = new Uom
                (
                    id: 0,
                    name: "N_0123456789_0123456789_0123456789_0123456789_0123456789",
                    description: ""
                );
                httpMessage.Content = GetWriteContent(uomToPost);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var errorDictionary = JsonConvert.DeserializeObject<Dictionary<string, ReadOnlyCollection<string>>>(jsonString);
                Assert.AreEqual(2, errorDictionary.Count);
                errorDictionary.TryGetValue("Name", out var value1);
                Assert.NotNull(value1);
                Assert.AreEqual(1, value1.Count);
                Assert.AreEqual("The field Name must be a string with a minimum length of 1 and a maximum length of 30.", value1[0]);
                errorDictionary.TryGetValue("Description", out var value2);
                Assert.NotNull(value2);
                Assert.AreEqual(2, value2.Count);
                Assert.AreEqual("The Description field is required.", value2[0]);
                Assert.AreEqual("The field Description must be a string with a minimum length of 1 and a maximum length of 100.", value2[1]);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomWriteService")]
        public async Task DeleteUomGoodClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testWriteClient = GetTestWriteClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Delete, $"{EndpointBase}/lrp/uoms/2", GoodClaimsList);
                var response = await testWriteClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
                using (var context = GetPropertyContext(connection))
                {
                    var uoms = await context.Uoms.Where(u => u.Description == "Inch").ToListAsync();
                    Assert.NotNull(uoms);
                    Assert.AreEqual(0, uoms.Count);

                    uoms = await context.Uoms.ToListAsync();
                    Assert.AreEqual(199, uoms.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
