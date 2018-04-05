using Microsoft.AspNetCore.Authorization;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Controllers;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns.UnitsOfMeasure
{
    [Authorize(Policy = "Scope")]
    [Authorize(Policy = "User")]
    public class UomReadController : ReadController<Uom>
    {
        public UomReadController
        (
            IReadRepository<Uom> repository,
            IPageLinksBuilder pageLinksBuilder
        ) : base(repository, pageLinksBuilder)
        {
        }
    }
}
