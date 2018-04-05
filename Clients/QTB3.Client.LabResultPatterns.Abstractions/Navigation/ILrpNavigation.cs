
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Abstractions.Navigation
{
    public interface ILrpNavigation
    {
        IReadOnlyList<Page> NavigationStack { get; }
        IReadOnlyList<Page> ModalStack { get; }
        Task PushAsync(Page page);
        Task PushModalAsync(Page page);
        Task<Page> PopAsync();
        Task<Page> PopModalAsync();
    }
}
