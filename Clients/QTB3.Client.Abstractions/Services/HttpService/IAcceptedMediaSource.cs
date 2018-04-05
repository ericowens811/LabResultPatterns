
using System.Net.Http.Headers;

namespace QTB3.Client.Abstractions.Services.HttpService
{
    public interface IAcceptedMediaSource
    {
        MediaTypeWithQualityHeaderValue Get();
    }
}
