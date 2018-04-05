using System;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Client.Common.Services.HttpService
{
    public class HttpServiceAddAcceptHeader : IHttpService
    {
        private readonly IAcceptedMediaSource _acceptedMedia;
        private readonly IHttpService _next;

        public HttpServiceAddAcceptHeader
        (
            IAcceptedMediaSource acceptedMedia,
            IHttpService next
        )
        {
            _acceptedMedia = acceptedMedia ?? throw new ArgumentNullException(nameof(acceptedMedia));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var acceptedMedia = _acceptedMedia.Get();
            request.Headers.Accept.Add(acceptedMedia);
            return await _next.SendAsync(request);
        }
    }
}
