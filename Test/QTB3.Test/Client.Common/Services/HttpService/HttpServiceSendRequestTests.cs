using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.HttpService
{
    [TestFixture]
    public class HttpServiceSendRequestTests
    {
        [Test]
        [Category("HttpServiceSendRequest")]
        public void Constructor()
        {
            var client = new Mock<IHttpClient>().Object;
            ConstructorTests<HttpServiceSendRequest>
                .For(typeof(IHttpClient))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { client }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("HttpServiceSendRequest")]
        public void SendAsync_RequestNull()
        {
            var client = new Mock<IHttpClient>(MockBehavior.Strict).Object;
            var uut = new HttpServiceSendRequest(client);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SendAsync(null)
            );
        }

        [Test]
        [Category("HttpServiceSendRequest")]
        public async Task SendAsync()
        {
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage();
            var client = new Mock<IHttpClient>(MockBehavior.Strict);
            client.Setup(n => n.SendAsync(request)).ReturnsAsync(response);
            var uut = new HttpServiceSendRequest(client.Object);
            var result = await uut.SendAsync(request);
        }
    }
}
