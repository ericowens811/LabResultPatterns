using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Utilities;

namespace QTB3.Api.LabResultPatterns.Utilities
{
    public class UrlBaseBuilder : IUrlBaseBuilder
    {
        public const string Host = "Host";
        public const string XForwardedHost = "X-Forwarded-Host";
        public const string XForwardedProto = "X-Forwarded-Proto";

        private readonly SfServices _sfServices;

        public UrlBaseBuilder(IOptions<SfServices> options)
        {
            if(options == null)
                throw new ArgumentNullException(nameof(options));
            _sfServices = options.Value;
        }

        public IUrlBases Build(HttpRequest request)
        {
            string scheme;
            string readServicePath;
            string writeServicePath;

            var host = request.Headers[XForwardedHost];           
            if (string.IsNullOrWhiteSpace(host)) // then this request did not come through the reverse proxy
            {
                var nonProxyHost = request.Headers[Host];
                // if the api is on a non-standard port then the headers.Host WILL include that port,
                // so this splits off the port
                // see https://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.host(v=vs.110).aspx
                host = nonProxyHost.ToString().Split(':')[0];
                scheme = request.Scheme;
                readServicePath = $"{_sfServices.ReverseProxyPort}{_sfServices.SfReadService}";
                writeServicePath = $"{_sfServices.ReverseProxyPort}{_sfServices.SfWriteService}";
            }
            else
            {
                scheme = request.Headers[XForwardedProto];
                // Here, the request has come through the reverse proxy and
                // XForwardedHost WILL include the reverse proxy port
                readServicePath = _sfServices.SfReadService;
                writeServicePath = _sfServices.SfWriteService;
            }
            
            // strip off any trailing slash from the request.Path
            var requestPath = request.Path.ToString().TrimEnd('/');

            return new UrlBases
            (
                $"{scheme}://{host}{readServicePath}{requestPath}",
                $"{scheme}://{host}{writeServicePath}{requestPath}"
            );
        }
    }
}
