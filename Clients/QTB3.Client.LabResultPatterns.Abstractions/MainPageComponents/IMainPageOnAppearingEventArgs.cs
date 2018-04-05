
using System.Threading.Tasks;

namespace QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents
{
    public interface IMainPageOnAppearingEventArgs
    {
        bool Handled { get; set; }
        TaskCompletionSource<object> Tcs { get; }
    }
}
