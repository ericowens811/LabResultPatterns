using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.LabResultPatterns.Common.LoginComponents.Exceptions;

namespace QTB3.Client.LabResultPatterns.Common.LoginComponents
{
    public class AzureB2CJwtTokenManager : IJwtTokenManager
    {
        // AzureB2C
        public const string B2CTenant = "CogitoModeloTenant.onmicrosoft.com";
        public const string B2ClientId = "6838061e-88c4-41a4-a2bd-c07237f7f687";
        public const string B2CResetPasswordPolicy = "B2C_1_SSPR";
        public const string B2CSignUpSignInPolicy = "B2C_1_SiUpIn";
        public const string B2CAuthorityBase = "https://login.microsoftonline.com/tfp/";
        public const string B2CRedirectPrefix = "msal";
        public const string B2CRedirectSuffix = "://auth";
        public const string B2CScope = "https://CogitoModeloTenant.onmicrosoft.com/cogtioserverone/labresultpatterns";

        private readonly UIParent _uiParent;
        private readonly string _policySignUpSignIn;
        private readonly string _authority;
        private readonly string _authorityPasswordReset;
        private readonly IPublicClientApplication _pca;

        // understanding scopes:
        // https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-access-tokens
        // azure ad b2c in xamarin forms:
        // https://github.com/Azure-Samples/active-directory-b2c-xamarin-native

        private readonly string[] _scopes =
        {
            B2CScope
        };

        public AzureB2CJwtTokenManager()
        {
            //TODO _uiParent is required for Android
            _uiParent = null;
            var authorityBase = $"{B2CAuthorityBase}{B2CTenant}/";
            _policySignUpSignIn = B2CSignUpSignInPolicy;
            _authority = $"{authorityBase}{_policySignUpSignIn}";
            _authorityPasswordReset = $"{authorityBase}{B2CResetPasswordPolicy}";

            //TODO new here?
            _pca = new PublicClientApplication(B2ClientId, _authority) {RedirectUri = $"{B2CRedirectPrefix}{B2ClientId}{B2CRedirectSuffix}"};
        }

        public void UpdateRedirectUri(string uri)
        {
            _pca.RedirectUri = uri;
        }

        public async Task<bool> RequestLoginAsync()
        {
            foreach (var user in _pca.Users)
            {
                _pca.Remove(user);
            }
            //ms-app://s-1-15-2-3388634293-103715313-3266902762-3262461205-4164087508-4155876753-1602774140/
            var userByPolicy = GetUserByPolicy(_pca.Users, _policySignUpSignIn);
            await _pca.AcquireTokenAsync(_scopes, userByPolicy, _uiParent);
            //TODO if we reach here, is there a chance login was not successful?
            return true;
        }

        public async Task<bool> ResetPasswordAsync()
        {
            await _pca.AcquireTokenAsync(_scopes, (IUser)null, UIBehavior.SelectAccount, String.Empty, null, _authorityPasswordReset, _uiParent);
            //TODO if we reach here, is there a chance login was not successful?
            return true;
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                var ar = await _pca.AcquireTokenSilentAsync(_scopes, GetUserByPolicy(_pca.Users, _policySignUpSignIn), _authority, false);
                return ar.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                throw new LoginRequestedException();
            }
            catch (Exception)
            {
                throw new TokenNotAvailableException();
            }
        }

        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                var userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }
            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            return decoded;
        }

        public JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }
    }
}
