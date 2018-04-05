using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using Xamarin.Forms.Xaml;

namespace QTB3.Client.LabResultPatterns.Common.LoginComponents
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ILoginPage, IOnAppearingAsync
    {

		public LoginPage ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            // returning true indicates that this method
            // handled the back button, and so, alone, returning true
            // disables the back button entirely
            return true;
        }

        public event Action<TaskCompletionSource<object>> LoginClickedEvent;

	    public async Task OnLoginClickedAsync()
	    {
	        if (LoginClickedEvent != null)
	        {
                var tcs = new TaskCompletionSource<object>();
                LoginClickedEvent.Invoke(tcs);
	            await tcs.Task;
	        }
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            await OnLoginClickedAsync();
        }

        public async Task OnAppearingAsync()
        {
            
        }

        protected override async void OnAppearing()
        {
            await OnAppearingAsync();
        }
    }
}
