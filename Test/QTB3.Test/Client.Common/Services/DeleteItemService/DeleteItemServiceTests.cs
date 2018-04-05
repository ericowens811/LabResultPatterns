using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Common.Services.DeleteItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.DeleteItemService
{
    [TestFixture]
    public class DeleteItemServiceTests
    {
        [Test]
        [Category("DeleteItemService")]
        public void Constructor()
        {
            var next = new Mock<IDeleteItemService<Uom>>().Object;
            ConstructorTests<DeleteItemService<Uom>>
                .For(typeof(IDeleteItemService<Uom>))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("DeleteItemService")]
        public void DeleteItemAsync_Id0()
        {
            var next = new Mock<IDeleteItemService<Uom>>(MockBehavior.Strict).Object;
            var uut = new DeleteItemService<Uom>(next);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>
            (
                async () => await uut.DeleteItemAsync(0)
            );
        }

        [Test]
        [Category("DeleteItemService")]
        public async Task DeleteItemAsync()
        {
            var next = new Mock<IDeleteItemService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.DeleteItemAsync(1001)).Returns(Task.CompletedTask);
            var uut = new DeleteItemService<Uom>(next.Object);
            await uut.DeleteItemAsync(1001);
        }

    }
}
