using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.LoginComponents.Exceptions;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Controllers
{
    public class LoginPageController: ILoginPageController
    {
        private readonly IJwtTokenManager _tokenManager;
        private readonly Func<ILoginPage> _createLoginPage;
        private readonly ILrpNavigation _navigation;

        private bool _hasLoggedInOnce = false;

        public LoginPageController
        (
            IMainPage mainPage,
            IJwtTokenManager tokenManager,
            Func<ILoginPage> createLoginPage,
            ILrpNavigation navigation
        )
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));     
            if(mainPage == null) throw new ArgumentNullException(nameof(mainPage));
            mainPage.OnAppearingCalledEvent += MainPageOnAppearingCalled; 
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _createLoginPage = createLoginPage ?? throw new ArgumentNullException(nameof(createLoginPage));
        }

        private async void MainPageOnAppearingCalled(IMainPageOnAppearingEventArgs args)
        {
            if (!_hasLoggedInOnce)
            {
                args.Handled = true;
                await ShowLoginAsync();
                args.Tcs.SetResult(null);
            }
        }

        private async Task InitiateLoginAsync()
        {
            var loginSuccess = false;
            // TODO
            // all of the paths out of this loop 
            //      -- HandleUnknownFailureAsync()
            //      -- ResetPasswordAsync()
            //      -- CheckForCanceledAsync
            // need to be implemented so as to return here after their
            // awaits complete...
            //
            while (!loginSuccess)
            {
                try
                {
                    loginSuccess = await _tokenManager.RequestLoginAsync();
                    if (!loginSuccess)
                    {
                        await HandleUnknownFailureAsync();
                    }
                    else
                    {
                        _hasLoggedInOnce = true;
                        await _navigation.PopModalAsync();
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains(LrpConstants.PasswordResetMessageContents))
                    {
                        await ResetPasswordAsync();
                    }
                    else
                    {
                        await CheckForCanceledAsync(e);
                        loginSuccess = true; // well, not really, but the user canceled the login
                    }                        // so we return them to the login page --
                                             // they'll need to click login again
                }
            }
        }

        private async Task ResetPasswordAsync()
        {
            try
            {
                var passwordReset = await _tokenManager.ResetPasswordAsync();
                if (!passwordReset)
                    await HandleUnknownFailureAsync();
                // else
                // The user has reset their password, and should be
                // redirected back to the remote OAuth authority login view
            }
            catch (Exception e)
            {
                await CheckForCanceledAsync(e);
            }
        }

        private async Task CheckForCanceledAsync(Exception e)
        {
            if ((e as MsalException)?.ErrorCode != LrpConstants.AuthenticationCanceledErrorCode)
                await HandleUnknownFailureAsync();
        }

        private async Task HandleUnknownFailureAsync()
        {
            // TODO
            // display a user message modal page here
            // user acknowledges, then back to login page
            // await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            throw new LoginFailedException();
        }

        public async Task ShowLoginAsync()
        {
            var loginPage = _createLoginPage();
            loginPage.LoginClickedEvent += LoginClickedAsync;
            await _navigation.PushModalAsync(loginPage as ContentPage);
        }

        private async void LoginClickedAsync(TaskCompletionSource<object> tcs)
        {
            await InitiateLoginAsync(); // could Throw
            tcs.SetResult(null);
        }
    }
}
