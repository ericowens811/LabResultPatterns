using QTB3.Model.Abstractions;

namespace QTB3.Api.Abstractions.Utilities
{
    public interface IPageLinksFormatter
    {
        string GetLinks<TItem>(string urlBase, IPage<TItem> page);
    }
}
