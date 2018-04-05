
using Microsoft.AspNetCore.Http;

namespace QTB3.Api.Abstractions.Utilities
{
    public interface IUrlBaseBuilder
    {
        IUrlBases Build(HttpRequest request);
    }
}
