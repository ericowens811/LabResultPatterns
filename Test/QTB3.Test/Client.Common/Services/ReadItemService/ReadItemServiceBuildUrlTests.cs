using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services.ReadItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.ReadItemService
{
    [TestFixture()]
    public class ReadItemServiceBuildUrlTests
    {
        [Test]
        [Category("ReadItemServiceBuildUrl")]
        public void Constructor()
        {
            var urlBuilder = new Mock<IItemReadUrlBuilder<Uom>>().Object;
            var next = new Mock<IReadItemServiceBuildRequest<Uom>>().Object;
            ConstructorTests<ReadItemServiceBuildUrl<Uom>>
                .For(typeof(IItemReadUrlBuilder<Uom>), typeof(IReadItemServiceBuildRequest<Uom>))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null urlBuilder")
                .Fail(new object[] { urlBuilder, null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { urlBuilder, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadItemServiceBuildUrl")]
        public void ReadItemAsync_Id0()
        {
            var urlBuilder = new Mock<IItemReadUrlBuilder<Uom>>(MockBehavior.Strict).Object;
            var next = new Mock<IReadItemServiceBuildRequest<Uom>>(MockBehavior.Strict).Object;
            var uut = new ReadItemServiceBuildUrl<Uom>(urlBuilder, next);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await uut.ReadItemAsync(0)
            );
        }

        [Test]
        [Category("ReadItemServiceBuildUrl")]
        public async Task ReadItemAsync()
        {
            var id = 1001;
            var expectedUom = new Uom();
            var url = "http://qtb3.com/a/b";
            var urlBuilder = new Mock<IItemReadUrlBuilder<Uom>>(MockBehavior.Strict);
            urlBuilder.Setup(u => u.Build(id)).Returns(url);
            var next = new Mock<IReadItemServiceBuildRequest<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.ReadItemAsync(url)).ReturnsAsync(expectedUom);
            var uut = new ReadItemServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            var actualUom = await uut.ReadItemAsync(id);
            Assert.AreEqual(expectedUom, actualUom);
        }
    }
}

