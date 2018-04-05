using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.Common.Services.SaveItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;
using IHttpWriteService = QTB3.Client.Abstractions.Services.HttpService.IHttpWriteService;

namespace QTB3.Test.Client.Common.Services.SaveItemService
{
    [TestFixture]
    public class SaveItemBuildRequestTests
    {
        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public void Constructor()
        {
            var httpService = new Mock<IHttpWriteService>().Object;
            var serializer = new Mock<IContentSerializer>().Object;
            ConstructorTests<SaveItemServiceBuildRequest<Uom>>
                .For(typeof(IHttpWriteService), typeof(IContentSerializer))
                .Fail(new object[] { null, serializer }, typeof(ArgumentNullException), "Null httpService.")
                .Fail(new object[] { httpService, null }, typeof(ArgumentNullException), "Null serializer.")
                .Succeed(new object[] { httpService, serializer }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public void SaveItemAsync_BadUrl()
        {
            var url = "abc";
            var uom = new Uom();
            var httpService = new Mock<IHttpWriteService>().Object;
            var serializer = new Mock<IContentSerializer>().Object;
            var uut = new SaveItemServiceBuildRequest<Uom>(httpService, serializer);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await uut.SaveItemAsync(url, uom)
            );
        }

        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public void SaveItemAsync_NullItem()
        {
            var url = "http://qtb3.com/a/b";
            Uom uom = null;
            var httpService = new Mock<IHttpWriteService>().Object;
            var serializer = new Mock<IContentSerializer>().Object;
            var uut = new SaveItemServiceBuildRequest<Uom>(httpService, serializer);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SaveItemAsync(url, uom)
            );
        }

        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public void SaveItemAsync_Status500()
        {
            var url = "http://qtb3.com/a/b";
            var uom = new Uom();
            HttpRequestMessage sentMessage = null;
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var sequence = new MockSequence();
            var serializer = new Mock<IContentSerializer>();
            serializer
                .InSequence(sequence)
                .Setup(s => s.Serialize(It.IsAny<HttpRequestMessage>(), uom));
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .InSequence(sequence)
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(response)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            var uut = new SaveItemServiceBuildRequest<Uom>(httpService.Object, serializer.Object);
            var exception = Assert.ThrowsAsync<FailedRequestException>(
                async () => await uut.SaveItemAsync(url, uom)
            );
            Assert.AreEqual(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.AreEqual(HttpMethod.Post, sentMessage.Method);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
        }

        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public async Task SaveItemAsync_GoodPost()
        {
            var url = "http://qtb3.com/a/b";
            var uom = new Uom();
            HttpRequestMessage sentMessage = null;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var sequence = new MockSequence();
            var serializer = new Mock<IContentSerializer>();
            serializer
                .InSequence(sequence)
                .Setup(s => s.Serialize(It.IsAny<HttpRequestMessage>(), uom));
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .InSequence(sequence)
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(response)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            var uut = new SaveItemServiceBuildRequest<Uom>(httpService.Object, serializer.Object);
            await uut.SaveItemAsync(url, uom);
           
            Assert.AreEqual(HttpMethod.Post, sentMessage.Method);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
        }

        [Test]
        [Category("SaveItemServiceBuildRequest")]
        public async Task SaveItemAsync_GoodPut()
        {
            var url = "http://qtb3.com/a/b";
            var uom = new Uom().WithId(1001);
            HttpRequestMessage sentMessage = null;
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var sequence = new MockSequence();
            var serializer = new Mock<IContentSerializer>();
            serializer
                .InSequence(sequence)
                .Setup(s => s.Serialize(It.IsAny<HttpRequestMessage>(), uom));
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .InSequence(sequence)
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(response)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            var uut = new SaveItemServiceBuildRequest<Uom>(httpService.Object, serializer.Object);
            await uut.SaveItemAsync(url, uom);

            Assert.AreEqual(HttpMethod.Put, sentMessage.Method);
            Assert.AreEqual(url, sentMessage.RequestUri.ToString());
        }
    }
}
