using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Common.Services.ReadItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.ReadItemService
{
    [TestFixture]
    public class ReadItemServiceTests
    {
        [Test]
        [Category("ReadItemService")]
        public void Constructor()
        {
            var next = new Mock<IReadItemService<Uom>>().Object;
            ConstructorTests<ReadItemService<Uom>>
                .For(typeof(IReadItemService<Uom>))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadItemService")]
        public void ReadItemAsync_Id0()
        {
            var next = new Mock<IReadItemService<Uom>>(MockBehavior.Strict);
            var uut = new ReadItemService<Uom>(next.Object);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await uut.ReadItemAsync(0)
            );
        }

        [Test]
        [Category("ReadItemService")]
        public async Task ReadItemAsync()
        {
            var id = 1001;
            var expectedUom = new Uom();
            var next = new Mock<IReadItemService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.ReadItemAsync(id)).ReturnsAsync(expectedUom);
            var uut = new ReadItemService<Uom>(next.Object);
            var actualUom = await uut.ReadItemAsync(id);
            Assert.AreEqual(expectedUom, actualUom);
        }
    }
}
