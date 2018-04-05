using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace QTB3.Client.LabResultPatterns.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());


            //CogiStd.App.PCA.RedirectUri = "msal6838061e-88c4-41a4-a2bd-c07237f7f687://auth";
            //app.LoginWorkflowController.UpdateRedirectUri("msal6838061e-88c4-41a4-a2bd-c07237f7f687://auth");


            //TODO workaround for self-signed certs on iOS
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
		        (sender, cert, chain, sslPolicyErrors) =>
		        {
		            if (cert != null) System.Diagnostics.Debug.WriteLine(cert);
		            return true;
		        };

            return base.FinishedLaunching(app, options);
		}

	    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
	    {
	        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
	        return true;
	    }
    }
}
