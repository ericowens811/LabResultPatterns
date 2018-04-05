using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services.SaveItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.SaveItemService
{
    [TestFixture]
    public class SaveItemServiceBuildUrlTests
    {
        [Test]
        [Category("SaveItemServiceBuildUrl")]
        public void Constructor()
        {
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>().Object;
            var next = new Mock<ISaveItemServiceBuildRequest<Uom>>().Object;
            ConstructorTests<SaveItemServiceBuildUrl<Uom>>
                .For(typeof(IItemWriteUrlBuilder<Uom>), typeof(ISaveItemServiceBuildRequest<Uom>))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null urlBuilder.")
                .Fail(new object[] { urlBuilder, null }, typeof(ArgumentNullException), "Null next.")
                .Succeed(new object[] { urlBuilder, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("SaveItemServiceBuildUrl")]
        public void SaveItemAsync_ItemNull()
        {
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>(MockBehavior.Strict).Object;
            var next = new Mock<ISaveItemServiceBuildRequest<Uom>>(MockBehavior.Strict).Object;
            var uut = new SaveItemServiceBuildUrl<Uom>(urlBuilder, next);
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await uut.SaveItemAsync(null)
            );
        }

        [Test]
        [Category("SaveItemServiceBuildUrl")]
        public async Task SaveItemAsync_Id0()
        {
            var uom = new Uom();
            var url = "http://qtb3.com";
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>(MockBehavior.Strict);
            urlBuilder.Setup(n => n.Build()).Returns(url);
            var next = new Mock<ISaveItemServiceBuildRequest<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.SaveItemAsync(url, uom)).Returns(Task.CompletedTask);
            var uut = new SaveItemServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            await uut.SaveItemAsync(uom);
        }

        [Test]
        [Category("SaveItemServiceBuildUrl")]
        public async Task SaveItemAsync_Id_Non0()
        {
            var uom = new Uom().WithId(1001);
            var url = "http://qtb3.com";
            var urlBuilder = new Mock<IItemWriteUrlBuilder<Uom>>(MockBehavior.Strict);
            urlBuilder.Setup(n => n.Build(uom.Id)).Returns(url);
            var next = new Mock<ISaveItemServiceBuildRequest<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.SaveItemAsync(url, uom)).Returns(Task.CompletedTask);
            var uut = new SaveItemServiceBuildUrl<Uom>(urlBuilder.Object, next.Object);
            await uut.SaveItemAsync(uom);
        }
    }
}
