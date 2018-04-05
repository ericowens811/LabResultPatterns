using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Common.Controllers
{
    public class WriteController<TItem> : Controller where TItem: class, IId
    {
        public const string BadIdMatchMessage = "Put request id does not match supplied item.id";
        public const string BadIdMessage = "Request id < 1";
        public const string BadPostMessage = "Post body must include the item";

        protected readonly IWriteRepository<TItem> Repository;

        public WriteController
        (
            IWriteRepository<TItem> repository
        )
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual async Task<IActionResult> PostAsync([FromBody]TItem value)
        {
            if (value == null)
            {
                return BadRequest(BadPostMessage);
            }
            var newValue = await Repository.PostAsync(value).ConfigureAwait(false);
            // using string constant route name in generic controller is problematic
            // so not this: return CreatedAtRoute("GetOne", new { id = newValue.Id }, newValue);
            return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{newValue.Id}"),  newValue);
        }

        public virtual async Task<IActionResult> PutAsync(int id, [FromBody]TItem value)
        {
            if(value.Id != id)
            {
                return BadRequest(BadIdMatchMessage);
            }

            if (id < 1)
            {
                return BadRequest(BadIdMessage);
            }

            await Repository.PutAsync(value).ConfigureAwait(false);
            return NoContent();
        }

        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest(BadIdMessage);
            }
            var itemFoundAndDeleted = await Repository.DeleteAsync(id);
            if (itemFoundAndDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
