using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Linking;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.LinkService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Constants;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Common.ViewModels;

namespace QTB3.Client.LabResultPatterns.Common.MainPageComponents
{
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private readonly ILinkService _linkService;
        private readonly ILinks _links;

        private readonly ILinkTemplateLookup _linkTemplates;
        
        public MainPageViewModel
        (
            string pageTitle,
            ILinkService linkService,
            ILinks links,
            ILinkTemplateLookup linkTemplates
        )
        {
            Title = pageTitle;
            _linkService = linkService;
            _links = links;
            _linkTemplates = linkTemplates;
        }

        public async Task RefreshLinks()
        {
            var links = await _linkService.GetLinksAsync();
            PopulateLinkTemplates(links, _linkTemplates);

            //var linksArray = links.Split(',');
            //foreach (var link in linksArray)
            //{
            //    var match = Regex.Match(link, ClientConstants.LinksPattern);
            //    var url = match.Groups[ClientConstants.UrlGroup].Value;
            //    var rel = match.Groups[ClientConstants.RelGroup].Value;
            //    Enum.TryParse(rel, out RelTypes relEnum);
            //    if (relEnum == RelTypes.notfound)
            //    {
            //        throw new ArgumentOutOfRangeException(nameof(rel));
            //    }
            //    _links.SetUrl(relEnum, url);
            //}
        }

        public void PopulateLinkTemplates(string links, ILinkTemplateLookup linkTemplates)
        {
            var linkTemplateArray = links.Split(',');
            foreach (var linkTemplate in linkTemplateArray)
            {
                var match = Regex.Match(linkTemplate, ClientConstants.LinksPattern);
                var urlTemplate = match.Groups[ClientConstants.UrlGroup].Value;
                var rel = match.Groups[ClientConstants.RelGroup].Value;
                linkTemplates[rel] = urlTemplate;
            }
        }
    }
}
