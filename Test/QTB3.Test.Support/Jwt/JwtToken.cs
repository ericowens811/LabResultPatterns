using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

// make tokens: https://www.codeproject.com/Articles/1205160/ASP-NET-Core-Bearer-Authentication

namespace QTB3.Test.Support.Jwt
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            return key;
        }
    }

    public sealed class JwtToken
    {
        private readonly JwtSecurityToken _token;

        internal JwtToken(JwtSecurityToken token)
        {
            _token = token;
        }

        public DateTime ValidTo => _token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(_token);
    }

    public sealed class JwtTokenBuilder
    {
        public const string Subject = "Cogito Modelo";
        public const string ValidIssuer = "Fiver.Security.Bearer";
        public const string ValidAudience = "Fiver.Security.Bearer";
        public const string KeySource = "lilyandlunaaretwocooldogsoftheforest";
        public const string AltKeySource = "lilyandlunaaretwocooldogsoftheforestXXX";

        private SecurityKey _securityKey;
        private string _subject = "";
        private string _issuer = "";
        private string _audience = "";
        private readonly Dictionary<string, string> _claims = new Dictionary<string, string>();
        private int _expiryInMinutes = 5;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            _securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            _audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            _claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            _expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments(_securityKey, _subject, _issuer, _audience);

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, _subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(_claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: _issuer,
                              audience: _audience,
                              claims: claims,
                              expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                              signingCredentials: new SigningCredentials(
                                                        _securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }

        private void EnsureArguments
        (
            SecurityKey securityKey,
            string subject,
            string issuer,
            string audience
        )
        {
            if (securityKey == null)
                throw new ArgumentNullException(nameof(securityKey));

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));

            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException(nameof(issuer));

            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException(nameof(audience));
        }
    }

    public class TestTokenGenerator
    {
        public string CreateTestToken(List<TestClaim> claims)
        {
            var tokenBuilder = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(JwtTokenBuilder.KeySource))
                                .AddSubject(JwtTokenBuilder.Subject)
                                .AddIssuer(JwtTokenBuilder.ValidIssuer)
                                .AddAudience(JwtTokenBuilder.ValidAudience)                               
                                .AddExpiry(1);
            foreach (var claim in claims)
            {
                tokenBuilder.AddClaim(claim.Name, claim.Value);
            }
            var token = tokenBuilder.Build();
            return token.Value;
        }
    }
}

