using System;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.Common.Utilities
{
    public class UrlBases : IUrlBases
    {
        public string ReadUrl { get; private set; }
        public string WriteUrl { get; private set; }

        public UrlBases
        (
            string readUrl,
            string writeUrl
        )
        {
            if (!Uri.IsWellFormedUriString(readUrl, UriKind.Absolute)) throw new ArgumentException(nameof(readUrl));
            if (!Uri.IsWellFormedUriString(writeUrl, UriKind.Absolute)) throw new ArgumentException(nameof(writeUrl));
            ReadUrl = readUrl;
            WriteUrl = writeUrl;
        }
    }
}
