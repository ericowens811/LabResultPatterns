using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.HttpService
{
    [TestFixture]
    public class HttpRequestBuilderTests
    {
        [Test]
        [Category("HttpRequestBuilder")]
        public void Constructor()
        {
            var mockController = new Mock<IJwtTokenSource>().Object;
            var mockSerializer = new Mock<IContentSerializer>().Object;
            ConstructorTests<HttpRequestBuilder>
            .For(typeof(IJwtTokenSource), typeof(IContentSerializer))
            .Fail(new object[] { mockController, null }, typeof(ArgumentNullException), "Null Serializer")
            .Fail(new object[] { null, mockSerializer }, typeof(ArgumentNullException), "Null Controller")
            .Succeed(new object[] { mockController }, "Valid Constructor.");
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public void BuildNullMethod()
        {
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            var builder = new HttpRequestBuilder(controller, serializer);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await builder.BuildAsync(null, "http://localhost")
            );
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public void BuildBadUrl()
        {
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            var builder = new HttpRequestBuilder(controller, serializer);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await builder.BuildAsync(HttpMethod.Get, "")
            );
        }

        [Category("HttpRequestBuilder")]
        public void BuildWithContentNullMethod()
        {
            var item = new Uom();
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            var builder = new HttpRequestBuilder(controller, serializer);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await builder.BuildAsync(null, "http://localhost", item)
            );
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public void BuildWithContentBadUrl()
        {
            var item = new Uom();
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            var builder = new HttpRequestBuilder(controller, serializer);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await builder.BuildAsync(HttpMethod.Get, "", item)
            );
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public void BuildWithContentNullItem()
        {
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict).Object;
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            var builder = new HttpRequestBuilder(controller, serializer);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await builder.BuildAsync<Uom>(HttpMethod.Get, "http://localhost", null)
            );
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public async Task BuildValid()
        {
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict);
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict).Object;
            controller
                .Setup(c => c.GetTokenAsync())
                .Returns(Task.FromResult("TheToken"));
            var builder = new HttpRequestBuilder(controller.Object, serializer);
            var message = await builder.BuildAsync(HttpMethod.Get, "http://localhost");
            Assert.NotNull(message);
            Assert.AreEqual(HttpMethod.Get, message.Method);
            Assert.AreEqual("http://localhost/", message.RequestUri.ToString());
            Assert.AreEqual("Bearer TheToken", message.Headers.Single(h => h.Key == "Authorization").Value?.FirstOrDefault());
        }

        [Test]
        [Category("HttpRequestBuilder")]
        public async Task BuildWithContentValid()
        {
            var expectedItem = new Uom(0, "TheUom", "TheDescription");
            var controller = new Mock<IJwtTokenSource>(MockBehavior.Strict);
            var serializer = new Mock<IContentSerializer>(MockBehavior.Strict);
            controller
                .Setup(c => c.GetTokenAsync())
                .Returns(Task.FromResult("TheToken"));
            serializer
                .Setup(s => s.Serialize(It.IsAny<HttpRequestMessage>(), expectedItem));
            var builder = new HttpRequestBuilder(controller.Object, serializer.Object);
            var message = await builder.BuildAsync(HttpMethod.Post, "http://localhost", expectedItem);
            Assert.NotNull(message);
            Assert.AreEqual(HttpMethod.Post, message.Method);
            Assert.AreEqual("http://localhost/", message.RequestUri.ToString());
            Assert.AreEqual("Bearer TheToken", message.Headers.Single(h => h.Key == "Authorization").Value?.FirstOrDefault());
        }
    }
}
