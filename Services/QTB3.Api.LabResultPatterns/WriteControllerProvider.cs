using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns
{
    public class WriteControllerProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.Clear();
            feature.Controllers.Add(typeof(UomWriteController).GetTypeInfo());
        }
    }
}
