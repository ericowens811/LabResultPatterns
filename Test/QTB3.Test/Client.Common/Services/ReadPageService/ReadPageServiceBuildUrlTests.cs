using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services.ReadPageService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.ReadPageService
{
    [TestFixture]
    public class ReadPageServiceBuildUrlTests
    {
        [Test]
        [Category("ReadPageServiceBuildUrl")]
        public void Constructor()
        {
            var urlBuilder = new Mock<IPageUrlBuilder<Uom>>().Object;
            var next = new Mock<IReadPageService<Uom>>().Object;
            ConstructorTests<ReadPageServiceBuildUrl<Uom>>
                .For(typeof(IPageUrlBuilder<Uom>), typeof(IReadPageService<Uom>))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null urlBuilder")
                .Fail(new object[] { urlBuilder, null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { urlBuilder, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ReadPageServiceBuildUrl")]
        public async Task ReadPageAsync()
        {
            var url = "qtb3.com/a/b";
            var expectedData = new Mock<ICollectionPageData<Uom>>(MockBehavior.Strict).Object;
            var urlBuilder = new Mock<IPageUrlBuilder<Uom>>();
            urlBuilder.Setup(u => u.Build(null)).Returns(url);
            var next = new Mock<IReadPageService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.ReadPageAsync(url)).ReturnsAsync(expectedData);
            var uut = new ReadPageServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            var actualData = await uut.ReadPageAsync(null);
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        [Category("ReadPageServiceBuildUrl")]
        public async Task ReadPageAsync_SearchText()
        {
            var searchText = "zzz";
            var url = "qtb3.com/a/b";
            var expectedData = new Mock<ICollectionPageData<Uom>>(MockBehavior.Strict).Object;
            var urlBuilder = new Mock<IPageUrlBuilder<Uom>>();
            urlBuilder.Setup(u => u.Build(searchText)).Returns(url);
            var next = new Mock<IReadPageService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.ReadPageAsync(url)).ReturnsAsync(expectedData);
            var uut = new ReadPageServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            var actualData = await uut.ReadPageAsync(searchText);
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
