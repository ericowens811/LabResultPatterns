using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.LabResultPatterns.Abstractions;

namespace QTB3.Api.Common.Controllers
{
    [Authorize(Policy = "Scope")]
    [Authorize(Policy = "User")]
    [Route("")]
    public class LinkTemplatesController : Controller
    {
        private const string LinkTemplateHeader = "Link-Template";
        private readonly IUrlBaseBuilder _urlBaseBuilder;
        private readonly ILinkTemplatesBuilder _linkTemplatesBuilder;

        public LinkTemplatesController
        (
            IUrlBaseBuilder urlBaseBuilder,
            ILinkTemplatesBuilder linkTemplatesBuilder
        )
        {
            _urlBaseBuilder = urlBaseBuilder ?? throw new ArgumentNullException(nameof(urlBaseBuilder));
            _linkTemplatesBuilder = linkTemplatesBuilder ?? throw new ArgumentNullException(nameof(linkTemplatesBuilder));
        }

        [HttpHead]
        public async Task<IActionResult> HeadAsync()
        {
            var urlBases = _urlBaseBuilder.Build(Request);
            var linkTemplates = _linkTemplatesBuilder.Build(urlBases);
            Response.Headers.Add(LinkTemplateHeader, linkTemplates);
            return Ok();
        }
    }
}
