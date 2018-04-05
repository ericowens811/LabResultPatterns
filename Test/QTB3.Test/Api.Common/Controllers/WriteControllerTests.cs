using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Common.Controllers;
using QTB3.Model.Abstractions;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.Common.Controllers
{
    [TestFixture]
    public class WriteControllerTests
    {
        public class TestEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public void NullifyNavProps()
            {
            }
        }

        [Test]
        [Category("WriteController")]
        public void Constructor()
        {
            var repository = new Mock<IWriteRepository<TestEntity>>().Object;
            ConstructorTests<WriteController<TestEntity>>
            .For(typeof(IWriteRepository<TestEntity>))
            .Fail(new object[]{ null }, typeof(ArgumentNullException), "Null repostory.")
            .Succeed(new object[] { repository }, "Constructor args valid.")
            .Assert();
        }

        [Test]
        [Category("WriteController")]
        public async Task DeleteIdLessThan1()
        {
            var item = new TestEntity { Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            var controller = new WriteController<TestEntity>(mockRepository.Object);

            var actionResult = await controller.DeleteAsync(0) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(WriteController<TestEntity>.BadIdMessage, actionResult.Value);
        }

        [Test]
        [Category("WriteController")]
        public void DeleteItemWhenRepoThrows()
        {
            var item = new TestEntity { Id = 123, Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.DeleteAsync(123)).ThrowsAsync(new IOException());
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            Assert.ThrowsAsync<IOException>(async () => await controller.DeleteAsync(123));
        }

        [Test]
        [Category("WriteController")]
        public async Task DeleteItemNotFound()
        {
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.DeleteAsync(123)).Returns(Task.FromResult(false));
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            var result = await controller.DeleteAsync(123) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Test]
        [Category("WriteController")]
        public async Task DeleteSucceeds()
        {
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.DeleteAsync(123)).Returns(Task.FromResult(true));
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            var result = await controller.DeleteAsync(123) as NoContentResult;
            Assert.NotNull(result);
        }

        [Test]
        [Category("WriteController")]
        public async Task PutItemIdNotMatchItemId()
        {
            var item = new TestEntity { Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            var controller = new WriteController<TestEntity>(mockRepository.Object);

            var actionResult = await controller.PutAsync(456, item) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(WriteController<TestEntity>.BadIdMatchMessage, actionResult.Value);
        }

        [Test]
        [Category("WriteController")]
        public async Task PutItemIdLessThan1()
        {
            var item = new TestEntity { Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            var controller = new WriteController<TestEntity>(mockRepository.Object);

            var actionResult = await controller.PutAsync(0, item) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(WriteController<TestEntity>.BadIdMessage, actionResult.Value);
        }

        [Test]
        [Category("WriteController")]
        public async Task PutItemRepoThrows()
        {
            var item = new TestEntity { Id = 123, Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.PutAsync(item)).ThrowsAsync(new IOException());
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            Assert.ThrowsAsync<IOException>(async () => await controller.PutAsync(123, item));
        }

        [Test]
        [Category("WriteController")]
        public async Task PutItemSucceeds()
        {
            var item = new TestEntity { Id = 123, Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.PutAsync(item)).Returns(Task.CompletedTask);
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            var actionResult = await controller.PutAsync(123, item) as NoContentResult;
            Assert.NotNull(actionResult);
        }

        [Test]
        [Category("WriteController")]
        public async Task PostItemNoItem()
        {
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            var controller = new WriteController<TestEntity>(mockRepository.Object);

            var actionResult = await controller.PostAsync(null) as BadRequestObjectResult;
            Assert.NotNull(actionResult);
            var message = actionResult.Value as string;
            Assert.NotNull(message);
            Assert.AreEqual(WriteController<TestEntity>.BadPostMessage, actionResult.Value);
        }

        [Test]
        [Category("WriteController")]
        public void PostItemWhenRepoThrows()
        {
            var item = new TestEntity { Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.PostAsync(item)).ThrowsAsync(new IOException());
            var controller = new WriteController<TestEntity>(mockRepository.Object);
            Assert.ThrowsAsync<IOException>(async () => await controller.PostAsync(item));
        }

        [Test]
        [Category("WriteController")]
        public async Task PostItemSucceeds()
        {
            var item = new TestEntity { Name = "Ent" };
            var repoItem = new TestEntity { Id = 123, Name = "Ent" };
            var mockRepository = new Mock<IWriteRepository<TestEntity>>(MockBehavior.Strict);
            mockRepository.Setup(r => r.PostAsync(item)).Returns(Task.FromResult(repoItem));
            var controller =
                new WriteController<TestEntity>(mockRepository.Object)
                {
                    ControllerContext = { HttpContext = new DefaultHttpContext() }
                };

            var request = controller.HttpContext.Request;
            request.Scheme = "https";
            request.Host = new HostString("somehost");
            request.Path = "/ents";

            var actionResult = await controller.PostAsync(item) as CreatedResult;
            Assert.NotNull(actionResult);
            Assert.AreEqual("https://somehost/ents/123", actionResult.Location);
            var dataResult = actionResult.Value as TestEntity;
            Assert.NotNull(dataResult);
            Assert.AreEqual(123, dataResult.Id);
            Assert.AreEqual("Ent", dataResult.Name);
        }
    }
}
