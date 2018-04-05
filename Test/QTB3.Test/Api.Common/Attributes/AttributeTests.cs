using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using QTB3.Api.Common.Attributes;

namespace QTB3.Test.Api.Common.Attributes
{
    [TestFixture]
    public class AttributeTests
    {
        public const string ScopePath = "http://schemas.microsoft.com/identity/claims/scope";

        public ActionExecutingContext GetContext(bool modelStateInvalid)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ScopePath, "yyy zzz")
            }));
            var httpContext = new DefaultHttpContext { User = user };
            var modelState = new ModelStateDictionary();
            if (modelStateInvalid)
            {
                modelState.AddModelError("P1", "bad");
                modelState.AddModelError("P2", "worse");
            }
            var actionContext = new ActionExecutingContext(
                new ActionContext
                (
                    httpContext,
                    new RouteData(),
                    new ActionDescriptor(),
                    modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            return actionContext;
        }

        [Test]
        [Category("ValidateModelAttribute")]
        public void ValidateModelValid()
        {
            var actionContext = GetContext(false);
            var sa = new ValidateModelAttribute();
            sa.OnActionExecuting(actionContext);
            Assert.Null(actionContext.Result);
        }

        [Test]
        [Category("ValidateModelAttribute")]
        public void ValidateModelNotValid()
        {
            var actionContext = GetContext(true);
            var sa = new ValidateModelAttribute();
            sa.OnActionExecuting(actionContext);
            var result = actionContext.Result as BadRequestObjectResult;
            Assert.NotNull(result);
            var errorDictionary = result.Value as SerializableError;
            Assert.NotNull(errorDictionary);
            Assert.True(errorDictionary.ContainsKey("P1"));
            Assert.True(errorDictionary.ContainsKey("P2"));
            var p1 = errorDictionary["P1"] as string[];
            Assert.NotNull(p1);
            Assert.AreEqual(1, p1.Length);
            Assert.AreEqual("bad", p1[0]);
            var p2 = errorDictionary["P2"] as string[];
            Assert.NotNull(p2);
            Assert.AreEqual(1, p2.Length);
            Assert.AreEqual("worse", p2[0]);
        }
    }
}
