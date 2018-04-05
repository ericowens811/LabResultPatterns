using System;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.Common.Services.ReadPageService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.PageMakers;
using QTB3.Test.LabResultPatterns.Support.TestData;
using QTB3.Test.Support.ConstructorTesting;
using IHttpReadService = QTB3.Client.Abstractions.Services.HttpService.IHttpReadService;

namespace QTB3.Test.Client.Common.Services.ReadPageService
{
    [TestFixture]
    public class ReadPageServiceBuildRequestTests
    {
        [Test]
        [Category("ReadPageServiceBuildRequest")]
        public void Constructor()
        {
            var httpService = new Mock<IHttpReadService>().Object;
            var deserializer = new Mock<IContentDeserializer>().Object;
            ConstructorTests<ReadPageServiceBuildRequest<Uom>>
                .For(typeof(IHttpReadService), typeof(IContentDeserializer))
                .Fail(new object[] { null, deserializer }, typeof(ArgumentNullException), "Null httpService")
                .Fail(new object[] { httpService, null }, typeof(ArgumentNullException), "Null deserializer")
                .Succeed(new object[] { httpService, deserializer }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadPageServiceBuildRequest")]
        public void ReadPageAsync_BadUrl()
        {
            var httpService = new Mock<IHttpReadService>(MockBehavior.Strict).Object;
            var deserializer = new Mock<IContentDeserializer>(MockBehavior.Strict).Object;
            var uut = new ReadPageServiceBuildRequest<Uom>(httpService, deserializer);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await uut.ReadPageAsync("abc")
            );
        }

        [Test]
        [Category("ReadPageServiceBuildRequest")]
        public void ReadPageAsync_HttpStatus500()
        {
            var url = "http://qtb3.com/a/b";
            HttpRequestMessage sentMessage = null;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpService = new Mock<IHttpReadService>(MockBehavior.Strict);
            var deserializer = new Mock<IContentDeserializer>(MockBehavior.Strict);
            httpService
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);

            var uut = new ReadPageServiceBuildRequest<Uom>(httpService.Object, deserializer.Object);
            var exception = Assert.ThrowsAsync<FailedRequestException>(
                async () => await uut.ReadPageAsync(url)
            );
            Assert.AreEqual(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.NotNull(sentMessage);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
            Assert.AreEqual(sentMessage.Method, HttpMethod.Get);
        }

        [Test]
        [Category("ReadPageServiceBuildRequest")]
        public async Task ReadPageAsync()
        {
            var url = "http://abc.com/api";
            var page = PageMaker.GetExpectedPage(
                data: UomTestData.GetUomsArray(),
                searchText: string.Empty,
                skip: 0,
                take: 20
            );
            var expectedResponse = CollectionPageHttpResponseMaker.GetExpectedResponse(page, url);
            var expectedData = CollectionPageDataMaker.GetExpectedPage(page, url);
            HttpRequestMessage sentMessage = null;
            var httpService = new Mock<IHttpReadService>(MockBehavior.Strict);
            var deserializer = new Mock<IContentDeserializer>(MockBehavior.Strict);
            var sequence = new MockSequence();
            httpService
                .InSequence(sequence)
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(expectedResponse)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            deserializer
                .InSequence(sequence)
                .Setup(d => d.DeserializeAsync<ImmutableList<Uom>>(expectedResponse.Content as StringContent))
                .ReturnsAsync(expectedData.Items as ImmutableList<Uom>);

            var uut = new ReadPageServiceBuildRequest<Uom>(httpService.Object, deserializer.Object);
            var actualData= await uut.ReadPageAsync(url);
            Assert.NotNull(sentMessage);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
            Assert.AreEqual(sentMessage.Method, HttpMethod.Get);
            Assert.True(CollectionPageDataEqual.Check(expectedData, actualData, UomEqual.Check));
        }
    }
}
