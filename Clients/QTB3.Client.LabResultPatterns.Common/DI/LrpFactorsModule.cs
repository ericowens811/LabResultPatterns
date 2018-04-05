using Autofac;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.Configuration;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Client.LabResultPatterns.Common.Configuration;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;

namespace QTB3.Client.LabResultPatterns.Common.DI
{
    public class LrpFactorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(r => new ApiEndpoint("https://lrpcluster.westus.cloudapp.azure.com:81/api"))
                .As(typeof(IEndPoint)).SingleInstance();

            builder.RegisterType<LrpHttpClient>().As<IHttpReadClient>().SingleInstance();
            builder.RegisterType<LrpHttpClient>().As<IHttpWriteClient>().SingleInstance();

            builder.RegisterType<AzureB2CJwtTokenManager>().As<IJwtTokenManager>().SingleInstance();
            builder.RegisterType<AzureB2CJwtTokenManager>().As<IJwtTokenSource>().SingleInstance();

        }
    }
}
 