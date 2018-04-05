using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.Common.Controllers
{
    [Authorize(Policy = "Scope")]
    [Authorize(Policy = "User")]
    [Route("")]
    public class ApiRootHeadController : Controller
    {
        public const string LinkHeaderValue = "Link";
        private readonly IRootLinksBuilder _linksBuilder;

        public ApiRootHeadController
        (
            IRootLinksBuilder linksBuilder
        )
        {
            _linksBuilder = linksBuilder ?? throw new ArgumentNullException(nameof(linksBuilder));
        }

        [HttpHead]
        public async Task<IActionResult> HeadAsync()
        {
            var links = _linksBuilder.Build(Request);
            Response.Headers.Add(LinkHeaderValue, links);
            return Ok();
        }

    }
}
