using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services.DeleteItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.DeleteItemService
{
    [TestFixture]
    public class DeleteItemServiceBuildUrlTestscs
    {
        [Test]
        [Category("DeleteItemServiceBuildUrl")]
        public void Constructor()
        {
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>().Object;
            var next = new Mock<IDeleteItemServiceBuildRequest>().Object;
            ConstructorTests<DeleteItemServiceBuildUrl<Uom>>
                .For(typeof(IItemWriteUrlBuilder<Uom>), typeof(IDeleteItemServiceBuildRequest))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null urlBuilder.")
                .Fail(new object[] { urlBuilder, null }, typeof(ArgumentNullException), "Null next.")
                .Succeed(new object[] { urlBuilder, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("DeleteItemServiceBuildUrl")]
        public void  DeleteItemAsync_Id0()
        {
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>(MockBehavior.Strict).Object;
            var next = new Mock<IDeleteItemServiceBuildRequest>(MockBehavior.Strict).Object;
            var uut = new DeleteItemServiceBuildUrl<Uom>(urlBuilder, next);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await uut.DeleteItemAsync(0)
            );
        }

        [Test]
        [Category("DeleteItemServiceBuildUrl")]
        public async Task DeleteItemAsync()
        {
            var url = "http://qtb3.com";
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>(MockBehavior.Strict);
            urlBuilder.Setup(n => n.Build(1001)).Returns(url);
            var next = new Mock<IDeleteItemServiceBuildRequest>(MockBehavior.Strict);
            next.Setup(n => n.DeleteItemAsync(url)).Returns(Task.CompletedTask);
            var uut = new DeleteItemServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            await uut.DeleteItemAsync(1001);
        }

    }
}
