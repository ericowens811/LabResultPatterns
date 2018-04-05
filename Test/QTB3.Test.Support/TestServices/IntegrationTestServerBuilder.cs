using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace QTB3.Test.Support.TestServices    
{
    public class IntegrationTestServerBuilder
    {
        public TestServer CreateServer<TStartup>() where TStartup : class
        {
            var server = new TestServer
            (
                new WebHostBuilder()
                    .UseStartup<TStartup>()
                    .UseUrls("") // url has no effect on TestServer
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        var env = builderContext.HostingEnvironment;
                        config.SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    })
            );
            return server;
        }
    }
}
