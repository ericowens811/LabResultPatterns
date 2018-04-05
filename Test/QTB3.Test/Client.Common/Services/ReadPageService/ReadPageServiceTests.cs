using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Common.Services.ReadPageService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.ReadPageService
{
    [TestFixture]
    public class ReadPageServiceTests
    {
        [Test]
        [Category("ReadPageService")]
        public void Constructor()
        {
            var next = new Mock<IReadPageService<Uom>>().Object;
            ConstructorTests<ReadPageService<Uom>>
                .For(typeof(IReadPageService<Uom>))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadPageService")]
        public void ReadPageAsync_BadUrl()
        {
            var next = new Mock<IReadPageService<Uom>>(MockBehavior.Strict);
            var uut = new ReadPageService<Uom>(next.Object);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await uut.ReadPageAsync("abc")
            );
        }

        [Test]
        [Category("ReadPageService")]
        public async Task ReadPageAsync()
        {
            var url = "http://qtb3.com/a/b";
            var expectedData = new Mock<ICollectionPageData<Uom>>(MockBehavior.Strict).Object;
            var next = new Mock<IReadPageService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.ReadPageAsync(url)).ReturnsAsync(expectedData);
            var uut = new ReadPageService<Uom>(next.Object);
            var actualData = await uut.ReadPageAsync(url);
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
