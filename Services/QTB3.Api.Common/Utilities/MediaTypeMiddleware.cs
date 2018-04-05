using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.Common.Utilities
{
    public class MediaTypeMiddleware
    {
        public const string AcceptHeaderValue = "Accept";
        private readonly RequestDelegate _next;
        private readonly ISupportedMedia _supportedMedia;

        public MediaTypeMiddleware(RequestDelegate next, ISupportedMedia supportedMedia)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _supportedMedia = supportedMedia ?? throw new ArgumentNullException(nameof(supportedMedia));
        }

        public async Task Invoke(HttpContext context)
        {
            // short circuit if the Accepted type can not be satisfied
            if (!_supportedMedia.Types.Contains(context.Request.Headers[AcceptHeaderValue]))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
                return; // short circuit
            }

            await _next(context);
        }

    }

    public static class MediaTypeMiddlewareExtensions
    {
        public static IApplicationBuilder UseMediaTypeMiddleware
        (
            this IApplicationBuilder builder
        )
        {
            return builder.UseMiddleware<MediaTypeMiddleware>();
        }
    }
}
