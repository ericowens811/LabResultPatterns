using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Test.Support.Jwt;

namespace QTB3.Test.LabResultPatterns.Support.Jwt
{
    public class TestJwtTokenManager : IJwtTokenManager
    {
        private readonly TestTokenGenerator _tokenGenerator = new TestTokenGenerator();
        private bool _shouldRequestRelogin;

        public void UpdateRedirectUri(string uri)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RequestLoginAsync()
        {
            return true;
        }

        public async Task<bool> ResetPasswordAsync()
        {
            throw new NotImplementedException();
        }

        public void SetNextGetTokenToRequestRelogin()
        {
            _shouldRequestRelogin = true;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_shouldRequestRelogin)
            {
                _shouldRequestRelogin = false;
                throw new MsalUiRequiredException("", "");
            }

            var claimsList = new List<TestClaim>
            {
                new TestClaim {Name= "http://schemas.microsoft.com/identity/claims/scope", Value="labresultpatterns" },
                new TestClaim {Name= "jobTitle", Value="Advanced" }
            };

            return _tokenGenerator.CreateTestToken(claimsList);
        }
    }
}
