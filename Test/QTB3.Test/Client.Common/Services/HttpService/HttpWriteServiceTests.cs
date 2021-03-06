﻿using System;
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
    public class HttpWriteServiceTests
    {
        [Test]
        [Category("HttpWriteService")]
        public void Constructor()
        {
            var next = new Mock<IHttpService>().Object;
            ConstructorTests<HttpWriteService>
                .For(typeof(IHttpService))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("HttpWriteService")]
        public void SendAsync_RequestNull()
        {
            var next = new Mock<IHttpService>(MockBehavior.Strict).Object;
            var uut = new HttpWriteService(next);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SendAsync(null)
            );
        }

        [Test]
        [Category("HttpWriteService")]
        public async Task SendAsync()
        {
            var request = new HttpRequestMessage();
            var response = new HttpResponseMessage();
            var next = new Mock<IHttpService>(MockBehavior.Strict);
            next.Setup(n => n.SendAsync(request)).ReturnsAsync(response);
            var uut = new HttpWriteService(next.Object);
            var result = await uut.SendAsync(request);
        }
    }
}
