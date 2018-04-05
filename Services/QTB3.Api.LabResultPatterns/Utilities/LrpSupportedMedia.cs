using System.Collections.Generic;
using System.Collections.Immutable;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.LabResultPatterns.Utilities
{
    public class LrpSupportedMedia: ISupportedMedia
    {
        public const string LrpMediaTypeV1 = "application/vnd.lrp.v1+json";
        public const string LrpMediaTypeV2 = "application/vnd.lrp.v2+json";
        public const string LrpMediaTypeV3 = "application/vnd.lrp.v3+json";

        public ImmutableList<string> Types { get; }

        public LrpSupportedMedia()
        {
            Types = new List<string>
            {
                LrpMediaTypeV1,
                LrpMediaTypeV2
            }.ToImmutableList();
        }
    }
}
