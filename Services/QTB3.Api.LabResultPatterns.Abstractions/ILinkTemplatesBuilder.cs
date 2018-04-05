
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.LabResultPatterns.Abstractions
{
    public interface ILinkTemplatesBuilder
    {
        string Build(IUrlBases urlBases);
    }
}
