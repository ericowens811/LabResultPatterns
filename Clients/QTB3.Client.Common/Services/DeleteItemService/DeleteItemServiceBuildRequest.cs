using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Common.Services.Exceptions;

namespace QTB3.Client.Common.Services.DeleteItemService
{
    public class DeleteItemServiceBuildRequest : IDeleteItemServiceBuildRequest
    {
        private readonly IHttpService _httpService;

        public DeleteItemServiceBuildRequest
        (
            IHttpWriteService httpService
        )
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task DeleteItemAsync(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await _httpService.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // already deleted by another racer
                }
                else
                {
                    throw new FailedRequestException(response.StatusCode);
                }
            }
        }
    }
}
