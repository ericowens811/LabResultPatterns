using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using Xamarin.Forms.Xaml;

namespace QTB3.Client.LabResultPatterns.Common.MainPageComponents
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : IMainPage, IOnAppearingAsync
	{
	    public event Action<TaskCompletionSource<object>> UomWorkflowStartedEvent;
	    public event Action<IMainPageOnAppearingEventArgs> OnAppearingCalledEvent;

        public MainPage()
        {
            InitializeComponent ();
		}

	    public async void OnUomsClicked(object sender, EventArgs e)
	    {
	        await OnUomsClickedAsync();
	    }

	    public async Task OnUomsClickedAsync()
	    {
	        if (UomWorkflowStartedEvent != null)
	        {
	            var tcs = new TaskCompletionSource<object>();
	            UomWorkflowStartedEvent.Invoke(tcs);
	            await tcs.Task;
            }
	    }

        public async Task OnAppearingAsync()
        {
            if (OnAppearingCalledEvent != null)
            {
                var tcs = new TaskCompletionSource<object>();
                var args = new MainPageOnAppearingEventArgs(tcs);
                OnAppearingCalledEvent.Invoke(args);
                await tcs.Task;
            }
        }

        protected override async void OnAppearing()
        {
            await OnAppearingAsync();
        }
    }
}