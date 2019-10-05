using GiphyLibrary.Data;
using GiphyLibrary.Domain;
using GiphyLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiphyLibrary.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly IGiphyClient client;
        private readonly IBlobStorageQuery query;

        public AccountController(ILogger<AccountController> logger, IGiphyClient client, IBlobStorageQuery query)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.query = query ?? throw new ArgumentNullException(nameof(query));
        }

        [HttpGet("SavedGiphies/{id}")]
        public async Task<ActionResult<Giphy>> GetSavedGiphy(string id)
        {
            var result = await query.GetBlob<Giphy>(HttpContext.User.Identity.Name, id).ConfigureAwait(false);
         
            if (result == null)
            {
                return NotFound();
            }

            return new ObjectResult(new Giphy
            {
                Id = result.Id,
                Caption = result.Caption,
                OriginalUrl = result.OriginalUrl,
                Tags = result.Tags
            });
        }

        [HttpGet("SavedGiphies")]
        public async Task<ActionResult<IEnumerable<Giphy>>> GetSavedGiphies(IEnumerable<string> tags = null)
        {
            // TODO: make compatible with pagination
            tags = tags ?? Enumerable.Empty<string>();

            var result = await query.GetBlobsInContainer<Giphy>(User.Identity.Name).ConfigureAwait(false);

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result.Data);
        }

        [HttpPost("SaveGiphy/{id}")]
        public async Task<IActionResult> PostGiphy(string id)
        {
            return await PostGiphy(id, null);
        }

        [HttpPost("TagGiphy/{id}")]
        public async Task<IActionResult> PostGiphy(string id, [FromBody] string tag = null)
        {
            var username = HttpContext.User.Identity.Name;
            var giphy = await query.GetBlob<Giphy>(username, id);
            if (giphy == null)
            {
                var giphyResult = await client.GetGiphy(id);
                if (giphyResult != null)
                {
                    await query.UploadBlob(new Giphy
                    {
                        Id = id,
                        Caption = giphyResult.Data.Caption,
                        OriginalUrl = giphyResult.Data.Images.Original.Url,
                        Tags = tag != null
                            ? new List<string> { tag }
                            : new List<string>()
                    },
                    username, id);

                    return new CreatedResult(nameof(GetSavedGiphy), id);
                }
                else
                {
                    return NotFound();
                }
            }
            else if (tag != null && !giphy.Tags.Exists(t => t.Equals(tag)))
            {
                giphy.Tags.Add(tag);
                await query.UploadBlob(giphy, username, id);
            }

            return NoContent();
        }
    }
}
