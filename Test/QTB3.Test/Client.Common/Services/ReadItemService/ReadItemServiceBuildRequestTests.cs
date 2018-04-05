using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.Common.Services.ReadItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using IHttpReadService = QTB3.Client.Abstractions.Services.HttpService.IHttpReadService;

namespace QTB3.Test.Client.Common.Services.ReadItemService
{
    [TestFixture]
    public class ReadItemServiceBuildRequestTests
    {
        [Test]
        [Category("ReadItemServiceBuildRequest")]
        public void Constructor()
        {
            var httpService = new Mock<IHttpReadService>().Object;
            var deserializer = new Mock<IContentDeserializer>().Object;
            ConstructorTests<ReadItemServiceBuildRequest<Uom>>
                .For(typeof(IHttpReadService), typeof(IContentDeserializer))
                .Fail(new object[] { null, deserializer }, typeof(ArgumentNullException), "Null httpService")
                .Fail(new object[] { httpService, null }, typeof(ArgumentNullException), "Null deserializer")
                .Succeed(new object[] { httpService, deserializer }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadItemServiceBuildRequest")]
        public void ReadItemAsync_BadUrl()
        {
            var httpService = new Mock<IHttpReadService>(MockBehavior.Strict).Object;
            var deserializer = new Mock<IContentDeserializer>(MockBehavior.Strict).Object;
            var uut = new ReadItemServiceBuildRequest<Uom>(httpService, deserializer);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await uut.ReadItemAsync("abc")
            );
        }

        [Test]
        [Category("ReadItemServiceBuildRequest")]
        public void ReadItemAsync_HttpStatus500()
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

            var uut = new ReadItemServiceBuildRequest<Uom>(httpService.Object, deserializer.Object);
            var exception = Assert.ThrowsAsync<FailedRequestException>(
                async () => await uut.ReadItemAsync(url)
            );
            Assert.AreEqual(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.NotNull(sentMessage);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
            Assert.AreEqual(sentMessage.Method, HttpMethod.Get);
        }

        [Test]
        [Category("ReadItemServiceBuildRequest")]
        public async Task ReadItemAsync()
        {
            var expectedUom = new Uom().WithId(1001);
            var url = "http://qtb3.com/a/b";
            HttpRequestMessage sentMessage = null;
            var httpResponseMessage =
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("stringContent")
                };
            var httpService = new Mock<IHttpReadService>(MockBehavior.Strict);
            var deserializer = new Mock<IContentDeserializer>(MockBehavior.Strict);
            var sequence = new MockSequence();
            httpService
                .InSequence(sequence)
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            deserializer
                .InSequence(sequence)
                .Setup(d => d.DeserializeAsync<Uom>(httpResponseMessage.Content))
                .ReturnsAsync(expectedUom);

            var uut = new ReadItemServiceBuildRequest<Uom>(httpService.Object, deserializer.Object);
            var actualUom = await uut.ReadItemAsync(url);
            Assert.NotNull(sentMessage);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
            Assert.AreEqual(sentMessage.Method, HttpMethod.Get);
            Assert.AreEqual(expectedUom, actualUom);
        }


    }
}
