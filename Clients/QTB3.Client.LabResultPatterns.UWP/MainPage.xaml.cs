using Windows.Security.Authentication.Web;

namespace QTB3.Client.LabResultPatterns.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            var app = new QTB3.Client.LabResultPatterns.Common.App();
            app.UpdateRedirectUri(WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString());
            LoadApplication(app);
        }
    }
}