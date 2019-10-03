using GiphyLibrary.Data;
using GiphyLibrary.Domain;
using GiphyLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly AccountDbContext context;

        public AccountController(ILogger<AccountController> logger, IGiphyClient client, AccountDbContext context)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [HttpGet("SavedGiphies/{id}")]
        public ActionResult<Giphy> GetSavedGiphy(string id)
        {
            var result = context.Giphies
                .Where(g => g.User == HttpContext.User.Identity.Name && g.GiphyId == id)
                .FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return new ObjectResult(new Giphy
            {
                Id = result.Id,
                Caption = result.Tags.FirstOrDefault(),
                OriginalUrl = result.OriginalUrl,
                DownsizedUrl = result.DownsizedUrl,
                Tags = result.Tags.ToArray()
            });
        }

        [HttpGet("SavedGiphies")]
        public ActionResult<IEnumerable<Giphy>> GetSavedGiphies(IEnumerable<string> tags = null)
        {
            tags = tags ?? Enumerable.Empty<string>();

            var result = context.Giphies
                .Where(g => g.User == HttpContext.User.Identity.Name && g.Tags.Exists(t => tags.Contains(t)))
                .Select(g => new Giphy
                {
                    Id = g.Id,
                    Caption = g.Tags.FirstOrDefault(),
                    OriginalUrl = g.OriginalUrl,
                    DownsizedUrl = g.DownsizedUrl,
                    Tags = g.Tags.ToArray()
                }).ToList();

            if(result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        [HttpPost("SavedGiphies/{id}")]
        public async Task<IActionResult> PostGiphy(string id, string tag = null)
        {
            var username = HttpContext.User.Identity.Name;
            var giphy = context.Giphies
                .Where(g => g.User == username && g.GiphyId == id)
                .FirstOrDefault();
            
            if(giphy != null && tag != null)
            {
                giphy.Tags.Append(tag);
                context.Giphies.Update(giphy);
                return NoContent();
            }
            else if (giphy == null)
            {
                var newGiphy = await client.GetGiphy(id);
                await context.Giphies.AddAsync(new StoredGiphy
                {
                    GiphyId = id,
                    User = username,
                    OriginalUrl = newGiphy.Data.Images.Original.Url,
                    DownsizedUrl = newGiphy.Data.Images.Original.Url,
                    Tags = tag != null
                        ? new List<string> { tag }
                        : new List<string>()
                });
                return new CreatedResult(nameof(GetSavedGiphy), id);
            }

            return NoContent();
        }
    }
}
