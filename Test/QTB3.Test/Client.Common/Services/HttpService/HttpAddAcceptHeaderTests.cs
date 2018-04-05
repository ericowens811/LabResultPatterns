using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.HttpService
{
    [TestFixture]
    public class HttpAddAcceptHeaderTests
    {
        [Test]
        [Category("HttpServiceAddAcceptHeader")]
        public void Constructor()
        {
            var acceptedMedia = new Mock<IAcceptedMediaSource>().Object;
            var next = new Mock<IHttpService>().Object;
            ConstructorTests<HttpServiceAddAcceptHeader>
                .For(typeof(IAcceptedMediaSource), typeof(IHttpService))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null acceptedMedia")
                .Fail(new object[] { acceptedMedia, null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { acceptedMedia, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("HttpServiceAddAcceptHeader")]
        public void SendAsync_RequestNull()
        {
            var acceptedMedia = new Mock<IAcceptedMediaSource>(MockBehavior.Strict).Object;
            var next = new Mock<IHttpService>(MockBehavior.Strict).Object;
            var uut = new HttpServiceAddAcceptHeader(acceptedMedia, next);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SendAsync(null)
            );
        }

        [Test]
        [Category("HttpServiceAddAcceptHeader")]
        public async Task SendAsync()
        {
            var media = new MediaTypeWithQualityHeaderValue("application/vnd.lrp.v1+json");
            var acceptedMedia = new Mock<IAcceptedMediaSource>(MockBehavior.Strict);
            var request = new HttpRequestMessage();
            var expectedResponse = new HttpResponseMessage();
            var next = new Mock<IHttpService>(MockBehavior.Strict);
            var sequence = new MockSequence();
            acceptedMedia.InSequence(sequence).Setup(a => a.Get()).Returns(media);
            next.InSequence(sequence).Setup(n => n.SendAsync(request)).ReturnsAsync(expectedResponse);

            var uut = new HttpServiceAddAcceptHeader(acceptedMedia.Object, next.Object);
            var actualResponse = await uut.SendAsync(request);
            Assert.AreEqual(expectedResponse, actualResponse);
            Assert.AreEqual(media.MediaType, request.Headers.Accept.ToString());
        }
    }
}
