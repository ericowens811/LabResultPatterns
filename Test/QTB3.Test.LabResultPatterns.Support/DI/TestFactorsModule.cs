using Autofac;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.Configuration;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.LabResultPatterns.Common.Configuration;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.Jwt;
using QTB3.Test.LabResultPatterns.Support.TestStartups;
using QTB3.Test.Support.TestServices;

namespace QTB3.Test.LabResultPatterns.Support.DI
{
    public class TestFactorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(r => new ApiEndpoint("http://localhost"))
                .As(typeof(IEndPoint)).SingleInstance();

            var serverBuilder = new IntegrationTestServerBuilder();

            var testReadServer = serverBuilder.CreateServer<TestReadServiceStartup>();
            var testReadClient = testReadServer.CreateClient();

            var testWriteServer = serverBuilder.CreateServer<TestWriteServiceStartup>();
            var testWriteClient = testWriteServer.CreateClient();

            builder.Register(e => new TestHttpClient(testReadClient))
                .As<IHttpReadClient>().SingleInstance();
            builder.Register(e => new TestHttpClient(testWriteClient))
                .As<IHttpWriteClient>().SingleInstance();

            builder.RegisterType<TestJwtTokenManager>().As<IJwtTokenManager>().SingleInstance();
            builder.RegisterType<TestJwtTokenManager>().As<IJwtTokenSource>().SingleInstance();
        }
    }
}
