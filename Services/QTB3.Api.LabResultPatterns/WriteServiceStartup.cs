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
using QTB3.Api.Common.Attributes;
using QTB3.Api.LabResultPatterns.UnitsOfMeasure;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns
{
    public class WriteServiceStartup: CommonStartup
    {
        public WriteServiceStartup
        (
            IConfiguration configuration
        ) : base(configuration)
        {
        }

        protected override void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidateModelAttribute());
                options.OutputFormatters.RemoveType<JsonOutputFormatter>();
                options.OutputFormatters.Insert(0, new LrpOutputFormatter(new LrpSupportedMedia(), new JsonSerializerSettings(), ArrayPool<char>.Shared));
                options.ReturnHttpNotAcceptable = true;
            }).ConfigureApplicationPartManager(p =>
                p.FeatureProviders.Add(new WriteControllerProvider()));
            services.AddOptions();
        }

        protected override void ConfigureLrpServices(IServiceCollection services)
        {
            base.ConfigureLrpServices(services);
            services.AddScoped<IWriteRepository<Uom>, UomWriteRepository>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            app.UseMvc(routes =>
            {
                var putRoute = new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint("PUT")});
                var postRoute = new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("POST")});
                var deleteRoute = new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("DELETE")});
                routes.MapRoute("post", "lrp/uoms", new { controller = "UomWrite", action = "PostAsync" }, postRoute);
                routes.MapRoute("put", "lrp/uoms/{id}", new { controller = "UomWrite", action = "PutAsync" }, putRoute);
                routes.MapRoute("delete", "lrp/uoms/{id}", new { controller = "UomWrite", action = "DeleteAsync" }, deleteRoute);
            });
        }
    }
}
