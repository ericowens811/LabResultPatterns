using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.Configuration;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.TestBuilders;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.LinkService
{
    [TestFixture]
    public class ClientLinkServiceTests
    {
        [Test]
        [Category("ClientLinkService")]
        public void Constructor()
        {
            var apiEndPoint = new Mock<IEndPoint>().Object;
            var httpRequestBuilder= new Mock<IHttpRequestBuilder>().Object;
            var httpClient = new Mock<IHttpReadClient>().Object;

            ConstructorTests<QTB3.Client.Common.Services.LinkService.LinkService>
                .For(typeof(IEndPoint), typeof(IHttpRequestBuilder), typeof(IHttpReadClient))
                .Fail(new object[] { null, httpRequestBuilder, httpClient }, typeof(ArgumentNullException), "apiEndpoint")
                .Fail(new object[] { apiEndPoint, null, httpClient }, typeof(ArgumentNullException), "httpRequestBuilder")
                .Fail(new object[] { apiEndPoint, httpRequestBuilder, null }, typeof(ArgumentNullException), "httpClient")
                .Succeed(new object[] { apiEndPoint, httpRequestBuilder, httpClient }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ClientLinkService")]
        public void GetLinksAsync_Throws()
        {
            var url = "http://a.b";
            var expectedRequest = new HttpRequestMessage();
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
            QTB3.Client.Common.Services.LinkService.LinkService service = new ServiceTestBuilder<Uom>()
                .ApiEndPoint_GetUrl(url)
                .HttpRequestBuilder_BuildAsync(HttpMethod.Head, url, expectedRequest)
                .HttpClient_SendAsync(expectedRequest, expectedResponse);

            Assert.ThrowsAsync<LinksException>
            (
                async () => await service.GetLinksAsync()
            );
        }

        [Test]
        [Category("ClientLinkService")]
        public async Task GetLinksAsync()
        {
            var url = "http://a.b";
            var links = "someLinksString";
            var expectedRequest = new HttpRequestMessage();
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            expectedResponse.Headers.Add("Link-Template", links);
            QTB3.Client.Common.Services.LinkService.LinkService service = new ServiceTestBuilder<Uom>()
                .ApiEndPoint_GetUrl(url)
                .HttpRequestBuilder_BuildAsync(HttpMethod.Head, url, expectedRequest)
                .HttpClient_SendAsync(expectedRequest, expectedResponse);

            var response = await service.GetLinksAsync();
            Assert.AreEqual(links, response);
        }
    }
}
