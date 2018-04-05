using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QTB3.Api.LabResultPatterns;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Test.Support.Jwt;

namespace QTB3.Test.LabResultPatterns.Support.TestStartups
{
    public class TestWriteServiceStartup : WriteServiceStartup
    {
        public SqliteConnection Connection;

        public TestWriteServiceStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDbContextServices(IServiceCollection services)
        {
            Connection = new SqliteConnection("DataSource=devicedb;Mode=Memory;Cache=Shared");
            services.AddDbContext<PropertyContext>(options =>
                options.UseSqlite(Connection));
        }

        protected override void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = JwtTokenBuilder.ValidIssuer,
                            ValidAudience = JwtTokenBuilder.ValidAudience,
                            IssuerSigningKey =
                                JwtSecurityKey.Create(JwtTokenBuilder.KeySource)
                        };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("jobTitle", "Advanced"));
                options.AddPolicy("Scope", policy => policy.RequireClaim(Configuration["AzureAdB2C:ScopePath"], Configuration["AzureAdB2C:ScopeRequired"]));
            });
        }
    }
}
