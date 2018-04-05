using System.Threading.Tasks;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;

namespace QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders
{
    public interface IItemMvcBuilder<TItem>
    {
        Task BuildAddAsync(ILrpNavigation navigation);
        Task BuildEditAsync(ILrpNavigation navigation, TItem item);
    }
}
