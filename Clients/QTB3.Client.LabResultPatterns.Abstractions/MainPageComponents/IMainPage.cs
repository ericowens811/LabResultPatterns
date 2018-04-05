using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents
{
    public interface IMainPage
    {
        object BindingContext { get; set; }

        INavigation Navigation { get; }

        event Action<IMainPageOnAppearingEventArgs> OnAppearingCalledEvent;
        event Action<TaskCompletionSource<object>> UomWorkflowStartedEvent;

        Task OnUomsClickedAsync();

    }
}
