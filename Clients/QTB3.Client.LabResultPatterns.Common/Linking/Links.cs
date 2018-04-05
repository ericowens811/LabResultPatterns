using System;
using System.Collections.Generic;
using QTB3.Client.Abstractions.Linking;
using QTB3.Model.Abstractions;

namespace QTB3.Client.LabResultPatterns.Common.Linking
{
    public class Links : ILinks
    {
        private readonly Dictionary<RelTypes, string> _lookup = new Dictionary<RelTypes, string>();

        public string GetUrl(RelTypes rel)
        {
            if(!_lookup.ContainsKey(rel)) throw new ArgumentException(nameof(rel));
            return _lookup[rel];
        }

        public void SetUrl(RelTypes rel, string url)
        {
            if (rel == RelTypes.notfound) throw new ArgumentOutOfRangeException(nameof(rel));
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            if (_lookup.ContainsKey(rel))
            {
                _lookup[rel] = url;
            }
            else
            {
                _lookup.Add(rel, url);
            }
        }
    }
}
