
using Microsoft.AspNetCore.Http;

namespace QTB3.Api.Abstractions.Utilities
{
    public interface IRootLinksBuilder
    {
        string Build(HttpRequest reques);
    }
}
