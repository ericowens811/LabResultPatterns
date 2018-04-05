using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Common.Utilities
{
    public class PageLinksBuilder : IPageLinksBuilder
    {
        private readonly IUrlBaseBuilder _urlBaseBuilder;
        private readonly IPageLinksFormatter _linksFormatter;

        public PageLinksBuilder
        (
            IUrlBaseBuilder urlBaseBuilder,
            IPageLinksFormatter linksFormatter
        )
        {
            _urlBaseBuilder = urlBaseBuilder ?? throw new ArgumentNullException(nameof(urlBaseBuilder));
            _linksFormatter = linksFormatter ?? throw new ArgumentNullException(nameof(linksFormatter));
        }

        public string Build<TItem>(IPage<TItem> page, HttpRequest request)
        {
            if(page == null) throw new ArgumentNullException(nameof(page));
            if(request == null) throw new ArgumentNullException(nameof(request));
            var urlBases = _urlBaseBuilder.Build(request);
            var links = _linksFormatter.GetLinks(urlBases.ReadUrl, page);
            return links;
        }
    }
}
