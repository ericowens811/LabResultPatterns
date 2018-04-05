using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services.DeleteItemService;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.DeleteItemService
{
    [TestFixture]
    public class DeleteItemServiceBuildRequestTests
    {
        [Test]
        [Category("DeleteItemServiceBuildRequest")]
        public void Constructor()
        {
            var httpService = new Mock<IHttpWriteService>().Object;
            ConstructorTests<DeleteItemServiceBuildRequest>
                .For(typeof(IHttpWriteService))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null httpService.")
                .Succeed(new object[] { httpService }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("DeleteItemServiceBuildRequest")]
        public async Task DeleteItemAsync()
        {
            var url = "https://qtb3.com";
            HttpRequestMessage sentMessage = null;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .Setup(s => s.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);
            
            var uut = new DeleteItemServiceBuildRequest(httpService.Object);
            await uut.DeleteItemAsync(url);
            Assert.AreEqual(HttpMethod.Delete, sentMessage.Method);
            Assert.AreEqual(url+"/", sentMessage.RequestUri.ToString());
        }

        [Test]
        [Category("DeleteItemServiceBuildRequest")]
        public void DeleteItemAsync_NotFound()
        {
            var url = "https://qtb3.com";
            HttpRequestMessage sentMessage = null;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .Setup(s => s.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);

            var uut = new DeleteItemServiceBuildRequest(httpService.Object);
            Assert.DoesNotThrowAsync(
                async () => await uut.DeleteItemAsync(url)
            );
            Assert.AreEqual(HttpMethod.Delete, sentMessage.Method);
            Assert.AreEqual(url + "/", sentMessage.RequestUri.ToString());
        }

        [Test]
        [Category("DeleteItemServiceBuildRequest")]
        public void DeleteItemAsync_InternalServerError()
        {
            var url = "https://qtb3.com";
            HttpRequestMessage sentMessage = null;
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpService = new Mock<IHttpWriteService>();
            httpService
                .Setup(s => s.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage)
                .Callback<HttpRequestMessage>((request) => sentMessage = request);

            var uut = new DeleteItemServiceBuildRequest(httpService.Object);
            Assert.ThrowsAsync<FailedRequestException>(
                async () => await uut.DeleteItemAsync(url)
            );
            Assert.AreEqual(HttpMethod.Delete, sentMessage.Method);
            Assert.AreEqual(url + "/", sentMessage.RequestUri.ToString());
        }
    }
}
