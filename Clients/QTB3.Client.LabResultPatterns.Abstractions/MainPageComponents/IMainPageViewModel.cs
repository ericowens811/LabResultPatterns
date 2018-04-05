
using System.Threading.Tasks;

namespace QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents
{
    public interface IMainPageViewModel
    {
        string Title { get; set; }
        Task RefreshLinks();
    }
}
