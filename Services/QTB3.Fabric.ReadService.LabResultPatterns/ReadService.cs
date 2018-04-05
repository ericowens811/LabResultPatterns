using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using QTB3.Api.LabResultPatterns;

namespace QTB3.Fabric.ReadService.LabResultPatterns
{
    internal sealed class ReadService : StatelessService
    {
        public ReadService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            FabricTelemetryInitializerExtension.SetServiceCallContext(this.Context);
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new HttpSysCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        url = $"{url}/api";
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting HttpSys on {url}");
                        var host = GetWebHostBuilder(serviceContext, url, listener)
                            .UseStartup<ReadServiceStartup>()
                            .UseApplicationInsights()
                            .Build();
                        return host;
                    }))
            };
        }

        private IWebHostBuilder GetWebHostBuilder(StatelessServiceContext serviceContext, string url, AspNetCoreCommunicationListener listener)
        {
            return new WebHostBuilder()
                .UseHttpSys()
                .ConfigureServices
                (
                    services =>
                    {
                        services.AddSingleton<StatelessServiceContext>(serviceContext);
                    }
                )
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                .UseUrls(url)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("azureKeyVault.json", false, true)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    var builtConfig = config.Build();

                    config.AddAzureKeyVault($"https://{builtConfig["azureKeyVault:vault"]}.vault.azure.net/",
                        builtConfig["azureKeyVault:clientId"],
                        builtConfig["azureKeyVault:clientSecret"]);

                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                });
        }
    }
}
