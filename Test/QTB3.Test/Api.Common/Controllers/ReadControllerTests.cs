using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Controllers;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.Common.Controllers
{
    [TestFixture]
    public class ReadControllerTests
    {
        public class TestEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public void NullifyNavProps()
            {
            }
        }

        public bool Check(TestEntity expected, TestEntity actual)
        {
            return
                expected.Id == actual.Id &&
                expected.Name == actual.Name;
        }

        [Test]
        [Category("ReadController")]
        public void Contructor()
        {
            var readRepository = new Mock<IReadRepository<TestEntity>>().Object;
            var pageLinksBuilder = new Mock<IPageLinksBuilder>().Object;
            ConstructorTests<ReadController<TestEntity>>
                .For(typeof(IReadRepository<TestEntity>), typeof(IPageLinksBuilder))
                .Fail(new object[] { readRepository, null }, typeof(ArgumentNullException), "Null PageLinksBuilder")
                .Fail(new object[] { null, pageLinksBuilder }, typeof(ArgumentNullException), "Null Respository")
                .Succeed(new object[] { readRepository, pageLinksBuilder }, "Constructor args valid")
                .Assert();
        }

        [Test]
        [Category("ReadController")]
        public async Task GetPageSkipLessThan0()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict).Object;

            var supportedMedia = new LrpSupportedMedia();

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller = new ReadController<TestEntity>(mockRepository, pageLinksBuilder);

            var actionResult = await controller.GetPageAsync("", -1, 1) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(ReadController<TestEntity>.BadSkipMessage, actionResult.Value);
        }

        [Test]
        [Category("ReadController")]
        public async Task GetPageTakeLessThan1()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict).Object;

            var supportedMedia = new LrpSupportedMedia();

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller = new ReadController<TestEntity>(mockRepository, pageLinksBuilder);

            var actionResult = await controller.GetPageAsync("", 0, 0) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(ReadController<TestEntity>.BadTakeMessage, actionResult.Value);
        }

        [Test]
        [Category("ReadController")]
        public void GetPageRepoThrows()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync("", 0, 20)).ThrowsAsync(new IOException());

            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder)
                {
                    ControllerContext = context
                };
            Assert.ThrowsAsync<IOException>(async () => await controller.GetPageAsync("", 0, 20));
        }

        [Test]
        [Category("ReadController")]
        public void GetPageLinksBuilderThrows()
        {
            var page = new Page<TestEntity>("",40,0,10,new List<TestEntity>());

            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync("", 0, 20)).ReturnsAsync(page);

            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext {HttpContext = new DefaultHttpContext()};
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>();
            pageLinksBuilder.Setup(p => p.Build(page, context.HttpContext.Request)).Throws(new Exception());

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder.Object)
                {
                    ControllerContext = context
                };
            Assert.ThrowsAsync<Exception>(async () => await controller.GetPageAsync("", 0, 20));
        }

        [Test]
        [Category("ReadController")]
        public async Task GetPageSucceeds()
        {
            var page = new Page<TestEntity>("", 40, 0, 10, new List<TestEntity>{ new TestEntity{Id=1001, Name="TheEntity"}});

            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync("", 0, 20)).ReturnsAsync(page);

            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>();
            pageLinksBuilder.Setup(p => p.Build(page, context.HttpContext.Request)).Returns("daLinks");

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder.Object)
                {
                    ControllerContext = context
                };
            var actionResult = await controller.GetPageAsync("", 0, 20) as OkObjectResult;

            Assert.NotNull(actionResult);
            var items = actionResult.Value as ImmutableList<TestEntity>;
            Assert.NotNull(items);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1001, items[0].Id);
            Assert.AreEqual("TheEntity", items[0].Name);
        }

        [Test]
        [Category("ReadController")]
        public async Task GetItemId0()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict).Object;

            var supportedMedia = new LrpSupportedMedia();

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller = new ReadController<TestEntity>(mockRepository, pageLinksBuilder);

            var actionResult = await controller.GetItemAsync(0) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(ReadController<TestEntity>.BadIdMessage, actionResult.Value);
        }

        [Test]
        [Category("ReadController")]
        public void GetItemRepoThrows()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync(123)).ThrowsAsync(new IOException());

            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder)
                {
                    ControllerContext = context
                };
            Assert.ThrowsAsync<IOException>(async () => await controller.GetItemAsync(123));
        }

        [Test]
        [Category("ReadController")]
        public async Task GetItemNotFound()
        {
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync(123)).ReturnsAsync(null as TestEntity);
            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder)
                {
                    ControllerContext = context
                };
            var result = await controller.GetItemAsync(123) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Test]
        [Category("ReadController")]
        public async Task GetItemFound()
        {
            var item = new TestEntity() {Id = 123, Name = "Uom123"};
            var mockRepository = new Mock<IReadRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.GetAsync(123)).ReturnsAsync(item);
            var supportedMedia = new LrpSupportedMedia();

            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.lrp.v1+json";

            var pageLinksBuilder = new Mock<IPageLinksBuilder>(MockBehavior.Strict).Object;

            var controller =
                new ReadController<TestEntity>(mockRepository.Object, pageLinksBuilder)
                {
                    ControllerContext = context
                };
            var actionResult = await controller.GetItemAsync(123) as OkObjectResult;
            Assert.NotNull(actionResult);
            var dataResult = actionResult.Value as TestEntity;
            Assert.NotNull(dataResult);
            Assert.AreEqual(123, dataResult.Id);
            Assert.AreEqual("Uom123", dataResult.Name);
        }
    }
}
