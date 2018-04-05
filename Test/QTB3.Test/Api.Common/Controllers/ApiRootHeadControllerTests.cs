using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Controllers;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.Common.Controllers
{
    [TestFixture]
    public class ApiRootHeadControllerTests
    {
        [Test]
        [Category("ApiHeadController")]
        public void Contructor()
        {
            var rootLinksBuilder = new Mock<IRootLinksBuilder>().Object;
            ConstructorTests<ApiRootHeadController>
                .For(typeof(IRootLinksBuilder))
                .Fail(new object[] { null }, typeof(ArgumentNullException), "Null RootLinksBuilder")
                .Succeed(new object[] { rootLinksBuilder }, "Constructor args valid")
                .Assert();
        }

        [Test]
        [Category("ApiHeadController")]
        public void LinksBuilderThrows()
        {
            var httpContext = new DefaultHttpContext();
            var context = new ControllerContext { HttpContext = httpContext };
            var rootLinksBuilder = new Mock<IRootLinksBuilder>();
            rootLinksBuilder.Setup(r => r.Build(context.HttpContext.Request)).Throws(new Exception());
            var controller = new ApiRootHeadController(rootLinksBuilder.Object){ControllerContext = context};
            Assert.ThrowsAsync<Exception>( async () => await controller.HeadAsync());
        }

        [Test]
        [Category("ApiHeadController")]
        public async Task ControllerSucceeds()
        {
            var httpContext = new DefaultHttpContext();
            var context = new ControllerContext { HttpContext = httpContext };
            var rootLinksBuilder = new Mock<IRootLinksBuilder>();
            rootLinksBuilder.Setup(r => r.Build(context.HttpContext.Request)).Returns("daLinks");
            var controller = new ApiRootHeadController(rootLinksBuilder.Object) { ControllerContext = context };
            var actionResult = await controller.HeadAsync() as OkResult;
            Assert.NotNull(actionResult);
        }

    }
}
