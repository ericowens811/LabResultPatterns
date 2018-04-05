using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Common.Services.SaveItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.SaveItemService
{
    [TestFixture]
    public class SaveItemServiceTests
    {
        [Test]
        [Category("SaveItemService")]
        public void Constructor()
        {
            var next = new Mock<ISaveItemService<Uom>>().Object;
            ConstructorTests<SaveItemService<Uom>>
                .For(typeof(ISaveItemService<Uom>))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("SaveItemService")]
        public void SaveItemAsync_NullItem()
        {
            var next = new Mock<ISaveItemService<Uom>>(MockBehavior.Strict).Object;
            var uut = new SaveItemService<Uom>(next);
            Assert.ThrowsAsync<ArgumentNullException>
            (
                async () => await uut.SaveItemAsync(null)
            );
        }

        [Test]
        [Category("SaveItemService")]
        public async Task SaveItemAsync()
        {
            var uom = new Uom();
            var next = new Mock<ISaveItemService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.SaveItemAsync(uom)).Returns(Task.CompletedTask);
            var uut = new SaveItemService<Uom>(next.Object);
            await uut.SaveItemAsync(uom);
        }
    }
}
