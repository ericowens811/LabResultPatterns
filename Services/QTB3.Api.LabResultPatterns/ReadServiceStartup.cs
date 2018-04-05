using System;
using System.Buffers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Utilities;
using QTB3.Api.LabResultPatterns.Abstractions;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns
{
    public class ReadServiceStartup: CommonStartup
    {
        public ReadServiceStartup
        (
            IConfiguration configuration
        ):base(configuration)
        {
        }

        protected override void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.OutputFormatters.RemoveType<JsonOutputFormatter>();
                    options.OutputFormatters.Insert(0, new LrpOutputFormatter(new LrpSupportedMedia(), new JsonSerializerSettings(), ArrayPool<char>.Shared));
                    options.ReturnHttpNotAcceptable = true;
                })
                .ConfigureApplicationPartManager(p =>
                    p.FeatureProviders.Add(new ReadControllerProvider()));
            services.AddOptions();
        }

        protected override void ConfigureLrpServices(IServiceCollection services)
        {
            base.ConfigureLrpServices(services);
            services.AddScoped<IUrlBaseBuilder, UrlBaseBuilder>();
            services.AddScoped<IReadRepository<Uom>, UomReadRepository>();
            services.AddScoped<IPageLinksFormatter, PageLinksFormatter>();
            services.AddScoped<IPageLinksBuilder, PageLinksBuilder>();
            services.AddScoped<ILinkTemplatesBuilder, LinkTemplatesBuilder>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            app.UseMvc(routes =>
            {
                var getRoute = new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") });
                routes.MapRoute("page", "lrp/uoms", new { controller = "UomRead", action = "GetPageAsync" }, getRoute);
                routes.MapRoute("item", "lrp/uoms/{id}", new { controller = "UomRead", action = "GetItemAsync" }, getRoute);
            });
        }
    }
}
