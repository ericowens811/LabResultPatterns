using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.Common.Controllers
{
    public class ReadController<TItem> : Controller
    {
        public const string BadSkipMessage = "Skip must not be < 0. ";
        public const string BadTakeMessage = "Take must not be < 1.";
        public const string BadIdMessage = "Id must not be < 1.";
        public const string LinkHeaderValue = "Link";
        public const string AcceptHeaderValue = "Accept";

        protected readonly IReadRepository<TItem> Repository;
        protected readonly IPageLinksBuilder PageLinksBuilder;

        public ReadController
        (
            IReadRepository<TItem> repository, 
            IPageLinksBuilder pageLinksBuilder
        )
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            PageLinksBuilder = pageLinksBuilder ?? throw new ArgumentNullException(nameof(pageLinksBuilder));
        }

        public async Task<IActionResult> GetPageAsync(string searchText, int skip, int take)
        {
            if (skip < 0 || take < 1)
            {
                var badMessage = string.Empty;
                if (skip < 0) badMessage += BadSkipMessage;
                if (take < 1) badMessage += BadTakeMessage;
                return BadRequest(badMessage);
            }

            var page = await Repository.GetAsync(searchText, skip, take).ConfigureAwait(false);
            var links = PageLinksBuilder.Build(page, Request);
            Response.Headers.Add(LinkHeaderValue, links);
            return Ok(page.Items);
        }

        public async Task<IActionResult> GetItemAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest(BadIdMessage);
            }

            var item = await Repository.GetAsync(id).ConfigureAwait(false);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
    }
}
