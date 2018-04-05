using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.LabResultPatterns
{
    [TestFixture]
    public class UrlBaseBuilderTests
    {
        [Test]
        [Category("UrlBaseBuilder")]
        public void Contructor()
        {
            var sfServices = new Mock<IOptions<SfServices>>().Object;

            ConstructorTests<UrlBaseBuilder>
                .For(typeof(IOptions<SfServices>))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null Options<SfServices>")
                .Succeed(new object[] { sfServices }, "Constructor args valid")
                .Assert();
        }

        [Test]
        [Category("UrlBaseBuilder")]
        public void NotThroughProxyNoPort()
        {
            var sfServices = new Mock<IOptions<SfServices>>(MockBehavior.Strict);
            sfServices.Setup(s => s.Value).Returns(new SfServices
            {
                ReverseProxyPort = ":32000",
                SfReadService = "/ReadService",
                SfWriteService = "/WriteService"
            });
            var headers = new Dictionary<string, StringValues>
            {
                { "Host", new StringValues("x.com") },
            };

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup(h => h.Headers).Returns(new HeaderDictionary(headers));
            httpRequest.Setup(h => h.Scheme).Returns("http");
            httpRequest.Setup(h => h.Path).Returns(new PathString("/a/b"));

            var urlBaseBuilder = new UrlBaseBuilder(sfServices.Object);

            var bases = urlBaseBuilder.Build(httpRequest.Object);
            Assert.AreEqual("http://x.com:32000/ReadService/a/b", bases.ReadUrl);
            Assert.AreEqual("http://x.com:32000/WriteService/a/b", bases.WriteUrl);
        }

        [Test]
        [Category("UrlBaseBuilder")]
        public void NotThroughProxyPort()
        {
            var sfServices = new Mock<IOptions<SfServices>>(MockBehavior.Strict);
            sfServices.Setup(s => s.Value).Returns(new SfServices
            {
                ReverseProxyPort = ":32000",
                SfReadService = "/ReadService",
                SfWriteService = "/WriteService"
            });
            var headers = new Dictionary<string, StringValues>
            {
                { "Host", new StringValues("x.com:8080") },
            };

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup(h => h.Headers).Returns(new HeaderDictionary(headers));
            httpRequest.Setup(h => h.Scheme).Returns("http");
            httpRequest.Setup(h => h.Path).Returns(new PathString("/a/b"));

            var urlBaseBuilder = new UrlBaseBuilder(sfServices.Object);

            var bases = urlBaseBuilder.Build(httpRequest.Object);
            Assert.AreEqual("http://x.com:32000/ReadService/a/b", bases.ReadUrl);
            Assert.AreEqual("http://x.com:32000/WriteService/a/b", bases.WriteUrl);
        }


        [Test]
        [Category("UrlBaseBuilder")]
        public void ThroughProxy()
        {
            var sfServices = new Mock<IOptions<SfServices>>(MockBehavior.Strict);
            sfServices.Setup(s => s.Value).Returns(new SfServices
            {
                ReverseProxyPort = ":32000",
                SfReadService = "/ReadService",
                SfWriteService = "/WriteService"
            });
            var headers = new Dictionary<string, StringValues>
            {
                { "X-Forwarded-Host", new StringValues("x.com:19080") },
                { "X-Forwarded-Proto", new StringValues("https") },

            };

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup(h => h.Headers).Returns(new HeaderDictionary(headers));
            httpRequest.Setup(h => h.Path).Returns(new PathString("/a/b"));

            var urlBaseBuilder = new UrlBaseBuilder(sfServices.Object);

            var bases = urlBaseBuilder.Build(httpRequest.Object);
            Assert.AreEqual("https://x.com:19080/ReadService/a/b", bases.ReadUrl);
            Assert.AreEqual("https://x.com:19080/WriteService/a/b", bases.WriteUrl);
        }

        [Test]
        [Category("UrlBaseBuilder")]
        public void ThroughProxyTrailingSlashOnPath()
        {
            var sfServices = new Mock<IOptions<SfServices>>(MockBehavior.Strict);
            sfServices.Setup(s => s.Value).Returns(new SfServices
            {
                ReverseProxyPort = ":32000",
                SfReadService = "/ReadService",
                SfWriteService = "/WriteService"
            });
            var headers = new Dictionary<string, StringValues>
            {
                { "X-Forwarded-Host", new StringValues("x.com:19080") },
                { "X-Forwarded-Proto", new StringValues("https") },
            };

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup(h => h.Headers).Returns(new HeaderDictionary(headers));
            httpRequest.Setup(h => h.Path).Returns(new PathString("/a/b/"));

            var urlBaseBuilder = new UrlBaseBuilder(sfServices.Object);

            var bases = urlBaseBuilder.Build(httpRequest.Object);
            Assert.AreEqual("https://x.com:19080/ReadService/a/b", bases.ReadUrl);
            Assert.AreEqual("https://x.com:19080/WriteService/a/b", bases.WriteUrl);
        }

    }
}
