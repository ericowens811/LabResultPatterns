using Microsoft.AspNetCore.Http;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Abstractions.Utilities
{
    public interface IPageLinksBuilder
    {
        string Build<TItem>(IPage<TItem> page,  HttpRequest request);
    }
}
