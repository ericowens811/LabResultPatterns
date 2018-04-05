using System;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using SmartFormat;

namespace QTB3.Client.Common.Services.UrlBuilding
{
    public class PageUrlBuilder<TItem> : IPageUrlBuilder<TItem>
    {
        private readonly string _templateKey;
        private readonly ILinkTemplateLookup _templateLookup;
        private readonly int skip = 0;
        private readonly int take;

        public PageUrlBuilder
        (
            string templateKey,
            ILinkTemplateLookup templateLookup, 
            int pageSize
        )
        {
            if(string.IsNullOrWhiteSpace(templateKey)) throw new ArgumentException(nameof(templateKey));
            _templateKey = templateKey;
            _templateLookup = templateLookup ?? throw new ArgumentNullException(nameof(templateLookup));
            if (pageSize < 1) throw new ArgumentException(nameof(pageSize));
            take = pageSize;
        }

        public string Build(string filter)
        {
            _templateLookup.TryGetValue(_templateKey, out var linkTemplate);
            return Smart.Format(linkTemplate, new { filter, skip, take });
        }
    }
}
