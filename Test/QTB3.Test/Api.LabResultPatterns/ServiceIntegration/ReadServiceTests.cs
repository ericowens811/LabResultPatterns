using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using NUnit.Framework;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.PageMakers;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.Db;

namespace QTB3.Test.Api.LabResultPatterns.ServiceIntegration
{
    [TestFixture]
    public class ReadServiceTests : ServiceTestBase
    {
        [Test]
        [Category("UomReadService")]
        public async Task TryGetSingleUomNoToken()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = new HttpRequestMessage(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2");
                httpMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(LrpSupportedMedia.LrpMediaTypeV2));
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task TryGetSingleUomGoodScopeBadUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2", GoodScopeBadUserClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task TryGetSingleUomBadScopeGoodUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2", BadScopeGoodUserClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task TryGetSingleUomBadScopeBadUserClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2", BadScopeBadUserClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task GetSingleUomBadMediaType()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2", GoodClaimsList, LrpSupportedMedia.LrpMediaTypeV3);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task GetSingleUomGoodClaimsList()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var httpMessage = GetRequestWithToken(HttpMethod.Get, $"{EndpointBase}/lrp/uoms/2", GoodClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.True(response.IsSuccessStatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var uomResult = JsonConvert.DeserializeObject<Uom>(jsonString);
                Assert.True(UomEqual.Check(testData[1], uomResult));
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("RootUrls")]
        public async Task GetRootUrls()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testReadClient = GetTestReadClient();
                var endpoint = $"{EndpointBase}";
                var httpMessage = GetRequestWithToken(HttpMethod.Head, endpoint, GoodClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.True(response.IsSuccessStatusCode);
                response.Headers.TryGetValues("Link", out var links);

                var linksArray = links?.FirstOrDefault()?.Split(',');
                if (linksArray != null)
                {
                    var dictionary = new Dictionary<RelTypes, string>();
                    var pattern = new Regex(@"\<(?'url'.*)\>.*rel=(?'rel'.*)$");
                    foreach (var link in linksArray)
                    {
                        var match = pattern.Match(link);
                        var url = match.Groups["url"].Value;
                        var rel = match.Groups["rel"].Value;
                        Enum.TryParse(rel, out RelTypes relEnum);
                        if (relEnum == RelTypes.notfound)
                        {
                            throw new ArgumentOutOfRangeException(nameof(rel));
                        }

                        dictionary.Add(relEnum, url);
                    }
                    var finalLinks = dictionary.ToImmutableDictionary();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task GetPageBadMediaType()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var skip = 0;
                var take = 20;
                var searchText = "";
                var endpoint = $"{EndpointBase}/lrp/uoms?searchText={WebUtility.UrlEncode(searchText)}&skip={skip}&take={take}";
                var httpMessage = GetRequestWithToken(HttpMethod.Get, endpoint, GoodClaimsList, LrpSupportedMedia.LrpMediaTypeV3);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task GetFirstPageOfUoms()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var skip = 0;
                var take = 20;
                var searchText = "";
                var endpoint = $"{EndpointBase}/lrp/uoms?searchText={WebUtility.UrlEncode(searchText)}&skip={skip}&take={take}";
                var httpMessage = GetRequestWithToken(HttpMethod.Get, endpoint, GoodClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.True(response.IsSuccessStatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var actualItems = JsonConvert.DeserializeObject<List<Uom>>(jsonString);
                var expectedPage = PageMaker.GetExpectedPage(testData, searchText, skip, take);
                Assert.True(PageEqual.CheckItemsOnly(expectedPage, actualItems, UomEqual.Check));
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        [Category("UomReadService")]
        public async Task GetSecondPageOfUoms()
        {
            var connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            try
            {
                var testData = UomTestData.GetUomsArray();
                new DbInitializer().Initialize(connection, GetPropertyContext, UomTestData.GetInitialData());
                var testReadClient = GetTestReadClient();
                var skip = 20;
                var take = 20;
                var searchText = "";
                var endpoint = $"{EndpointBase}/lrp/uoms?searchText={WebUtility.UrlEncode(searchText)}&skip={skip}&take={take}";
                var httpMessage = GetRequestWithToken(HttpMethod.Get, endpoint, GoodClaimsList);
                var response = await testReadClient.SendAsync(httpMessage);
                Assert.True(response.IsSuccessStatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var actualItems = JsonConvert.DeserializeObject<List<Uom>>(jsonString);
                var expectedPage = PageMaker.GetExpectedPage(testData, searchText, skip, take);
                Assert.True(PageEqual.CheckItemsOnly(expectedPage, actualItems, UomEqual.Check));
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
