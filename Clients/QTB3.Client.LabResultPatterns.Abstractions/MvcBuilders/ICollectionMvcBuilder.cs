
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Model.Abstractions;

namespace QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders
{
    public interface ICollectionMvcBuilder<TItem> where TItem: class, IEntity
    {
        Task BuildAsync(ILrpNavigation navigation, string pageTitle);
    }
}
