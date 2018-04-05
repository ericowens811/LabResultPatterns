
using System;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.Configuration;

namespace QTB3.Client.LabResultPatterns.Common.Configuration
{
    public class ApiEndpoint : IEndPoint
    {
        public string Url { get; }

        public ApiEndpoint(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            Url = url;
        }
    }
}
