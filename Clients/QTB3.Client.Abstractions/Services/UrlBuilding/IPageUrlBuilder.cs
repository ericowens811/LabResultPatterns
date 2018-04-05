
namespace QTB3.Client.Abstractions.Services.UrlBuilding
{
    public interface IPageUrlBuilder<TItem>
    {
        string Build(string filter);
    }
}
