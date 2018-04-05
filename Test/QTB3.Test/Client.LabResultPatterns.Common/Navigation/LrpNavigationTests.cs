using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.Navigation;
using Xamarin.Forms;

namespace QTB3.Test.Client.LabResultPatterns.Common.Navigation
{
    [TestFixture]
    public class LrpNavigationTests
    {
        [Test]
        [Category("LrpNavigation")]
        public void NavigationStack()
        {
            var expectedNavStack = new Mock<IReadOnlyList<Page>>(MockBehavior.Strict).Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.NavigationStack).Returns(expectedNavStack);
            var lrpNav = new LrpNavigation(mockNav.Object);
            Assert.AreEqual(expectedNavStack, lrpNav.NavigationStack);
        }

        [Test]
        [Category("LrpNavigation")]
        public void  ModalStack()
        {
            var expectedModalStack = new Mock<IReadOnlyList<Page>>(MockBehavior.Strict).Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.ModalStack).Returns(expectedModalStack);
            var lrpNav = new LrpNavigation(mockNav.Object);
            Assert.AreEqual(expectedModalStack, lrpNav.ModalStack);
        }

        [Test]
        [Category("LrpNavigation")]
        public async Task Push()
        {
            var page = new Mock<Page>().Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.PushAsync(page)).Returns(Task.CompletedTask);
            var lrpNav = new LrpNavigation(mockNav.Object);
            await lrpNav.PushAsync(page);
        }

        [Test]
        [Category("LrpNavigation")]
        public async Task PushModal()
        {
            var page = new Mock<Page>().Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.PushModalAsync(page)).Returns(Task.CompletedTask);
            var lrpNav = new LrpNavigation(mockNav.Object);
            await lrpNav.PushModalAsync(page);
        }

        [Test]
        [Category("LrpNavigation")]
        public async Task Pop()
        {
            var expectedPage = new Mock<Page>().Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.PopAsync()).ReturnsAsync(expectedPage);
            var lrpNav = new LrpNavigation(mockNav.Object);
            var actualPage = await lrpNav.PopAsync();
            Assert.AreEqual(expectedPage, actualPage);
        }

        [Test]
        [Category("LrpNavigation")]
        public async Task PopModal()
        {
            var expectedPage = new Mock<Page>().Object;
            var mockNav = new Mock<INavigation>();
            mockNav.Setup(m => m.PopModalAsync()).ReturnsAsync(expectedPage);
            var lrpNav = new LrpNavigation(mockNav.Object);
            var actualPage = await lrpNav.PopModalAsync();
            Assert.AreEqual(expectedPage, actualPage);
        }
    }
}
