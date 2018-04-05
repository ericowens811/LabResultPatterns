using QTB3.Api.Common.Repositories;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns.UnitsOfMeasure
{
    public class UomWriteRepository : WriteRepository<Uom>
    {
        public UomWriteRepository
        (
            PropertyContext context
        ) : base( context)
        {
        }
    }
}
