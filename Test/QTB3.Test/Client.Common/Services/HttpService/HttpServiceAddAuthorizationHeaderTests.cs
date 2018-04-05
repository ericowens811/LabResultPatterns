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
    public class HttpServiceAddAuthorizationHeaderTests
    {
        [Test]
        [Category("HttpServiceAddAuthorizationHeader")]
        public void Constructor()
        {
            var tokenSource = new Mock<IJwtTokenSource>().Object;
            var next = new Mock<IHttpService>().Object;
            ConstructorTests<HttpServiceAddAuthorizationHeader>
                .For(typeof(IJwtTokenSource), typeof(IHttpService))
                .Fail(new object[] {tokenSource, null}, typeof(ArgumentNullException), "Null next")
                .Fail(new object[] {null, next}, typeof(ArgumentNullException), "Null tokenSource")
                .Succeed(new object[] {tokenSource, next}, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("HttpServiceAddAuthorizationHeader")]
        public void SendAsync_RequestNull()
        {
            var tokenSource = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var next = new Mock<IHttpService>(MockBehavior.Strict).Object;
            var uut = new HttpServiceAddAuthorizationHeader(tokenSource, next);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SendAsync(null)
            );
        }

        [Test]
        [Category("HttpServiceAddAuthorizationHeader")]
        public async Task SendAsync()
        {
            var token = "123";
            var request = new HttpRequestMessage();
            var expectedResponse = new HttpResponseMessage();
            var tokenSource = new Mock<IJwtTokenSource>(MockBehavior.Strict);
            var next = new Mock<IHttpService>(MockBehavior.Strict);
            var sequence = new MockSequence();
            tokenSource.InSequence(sequence).Setup(t => t.GetTokenAsync()).ReturnsAsync(token);
            next.InSequence(sequence).Setup(n => n.SendAsync(request)).ReturnsAsync(expectedResponse);
            var uut = new HttpServiceAddAuthorizationHeader(tokenSource.Object, next.Object);

            var actualResponse = await uut.SendAsync(request);
            Assert.AreEqual(expectedResponse, actualResponse);
            Assert.AreEqual("Bearer 123", request.Headers.Authorization.ToString());
        }
    }
}
