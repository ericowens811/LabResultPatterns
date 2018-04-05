using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Utilities;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.LabResultPatterns.Contexts;

namespace QTB3.Api.LabResultPatterns
{
    public class CommonStartup
    {
        public CommonStartup
        (
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected virtual void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority =
                        $"https://login.microsoftonline.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
                    jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                    //jwtOptions.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = AuthenticationFailed
                    //};
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("jobTitle", "Advanced"));
                options.AddPolicy("Scope", policy => policy.RequireClaim(Configuration["AzureAdB2C:ScopePath"], Configuration["AzureAdB2C:ScopeRequired"]));
            });
        }

        protected virtual void ConfigureDbContextServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextPool<PropertyContext>(options =>
                options.UseSqlServer(connectionString));
        }

        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            // configured in subclass
        }

        protected virtual void ConfigureLrpServices(IServiceCollection services)
        {
            services.Configure<SfServices>(Configuration.GetSection("SfServices"));
            services.AddScoped<ISupportedMedia, LrpSupportedMedia>();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthenticationServices(services);
            ConfigureDbContextServices(services);
            ConfigureMvc(services);
            ConfigureLrpServices(services);
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseMediaTypeMiddleware();

            var loggingConfig = Configuration.GetSection("Logging");
            loggerFactory.AddConsole(loggingConfig);
            loggerFactory.AddDebug();
        }

        //private Task AuthenticationFailed(AuthenticationFailedContext arg)
        //{
        //    // For debugging purposes only!
        //    var s = $"AuthenticationFailed: {arg.Exception.Message}";
        //    arg.Response.ContentLength = s.Length;
        //    arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
        //    return Task.FromResult(0);
        //}
    }
}
