using System;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using SmartFormat;

namespace QTB3.Client.Common.Services.UrlBuilding
{
    public class ItemUrlBuilder<TItem> : IItemReadUrlBuilder<TItem>, IItemWriteUrlBuilder<TItem>
    {
        private readonly string _templateKey;
        private readonly ILinkTemplateLookup _templateLookup;

        public ItemUrlBuilder(string templateKey, ILinkTemplateLookup templateLookup)
        {
            if (string.IsNullOrWhiteSpace(templateKey)) throw new ArgumentException(nameof(templateKey));
            _templateKey = templateKey;
            _templateLookup = templateLookup ?? throw new ArgumentNullException(nameof(templateLookup));
        }

        public string Build(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException();
            _templateLookup.TryGetValue(_templateKey, out var linkTemplate);
            return Smart.Format(linkTemplate, new { id });
        }

        public string Build()
        {
            _templateLookup.TryGetValue(_templateKey, out var linkTemplate);
            return linkTemplate.Replace("/{id}", "");
        }
    }
}
