using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Common.Controllers;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns.UnitsOfMeasure
{
    [Authorize(Policy = "Scope")]
    [Authorize(Policy = "User")]
    public class UomWriteController: WriteController<Uom>
    {
        public UomWriteController(IWriteRepository<Uom> repository) : base(repository)
        {
        }
    }
}
