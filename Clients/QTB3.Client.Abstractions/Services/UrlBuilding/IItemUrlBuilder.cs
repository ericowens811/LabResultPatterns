
namespace QTB3.Client.Abstractions.Services.UrlBuilding
{
    public interface IItemUrlBuilder<TItem>
    {
        string Build(int id);
        string Build();
    }
}
