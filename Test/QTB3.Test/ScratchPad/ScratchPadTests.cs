using NUnit.Framework;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.LabResultPatterns.Common.DI;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using Xamarin.Forms;

namespace QTB3.Test.ScratchPad
{
    [TestFixture]
    public class ScratchPadTests //: SqlitePropertyContextTestBase
    {

        // ------------------------------------------------------------------
        //
        // Link Templates
        //
        // ------------------------------------------------------------------
        public class LinksLookupBuilder
        {
            public const string UrlGroup = "url";
            public const string RelGroup = "rel";
            public const string LinksPattern = @"\<(?'url'.*)\>;\s*rel=(?'rel'.*)$";

            public Dictionary<string, string> Build(string linkTemplates, Dictionary<string, string> lookup)
            {
                var linkTemplateArray = linkTemplates.Split(',');
                foreach (var linkTemplate in linkTemplateArray)
                {
                    var match = Regex.Match(linkTemplate, LinksPattern);
                    var urlTemplate = match.Groups[UrlGroup].Value;
                    var rel = match.Groups[RelGroup].Value;
                    lookup.TryAdd(rel, urlTemplate);
                }
                return lookup;
            }
        }


    }
}
