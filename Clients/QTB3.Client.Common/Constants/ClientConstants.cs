using System;
using System.Collections.Generic;
using System.Text;

namespace QTB3.Client.Common.Constants
{
    public class ClientConstants
    {
        public const string LinksPattern = @"\<(?'url'.*)\>;\s*rel=(?'rel'.*)$";
        public const string SkipTakePattern = @"skip=(?'skip'\d*).*take=(?'take'\d*)";
        public const string UrlGroup = "url";
        public const string RelGroup = "rel";
        public const string SkipGroup = "skip";
        public const string TakeGroup = "take";

    }
}
