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

        [HttpGet("SavedGiphies")]
        public IEnumerable<Giphy> GetSavedGiphies(IEnumerable<string> tags = null)
        {
            tags = tags ?? Enumerable.Empty<string>();

            return context.Giphies
                .Where(g => g.User == HttpContext.User.Identity.Name && g.Tags.Exists(t => tags.Contains(t)))
                .Select(g => new Giphy
                {
                    Id = g.Id,
                    Caption = g.Tags.FirstOrDefault(),
                    OriginalUrl = g.OriginalUrl,
                    DownsizedUrl = g.DownsizedUrl,
                    Tags = g.Tags.ToArray()
                });
        }

        [HttpPost("SavedGiphies/{id}")]
        public async Task PostGiphy(string id, [FromBody] string tag = "")
        {
            var username = HttpContext.User.Identity.Name;
            var giphy = context.Giphies
                .Where(g => g.User == username && g.GiphyId == id)
                .FirstOrDefault();
            
            if(giphy != null)
            {
                giphy.Tags.Append(tag);
                context.Giphies.Update(giphy);
            }
            else
            {
                var newGiphy = await client.GetGiphy(id);
                await context.Giphies.AddAsync(new StoredGiphy
                {
                    GiphyId = id,
                    User = username,
                    OriginalUrl = newGiphy.Data.Images.Original.Url,
                    DownsizedUrl = newGiphy.Data.Images.Original.Url,
                    Tags = new List<string> { tag }
                });
            }
        }
    }
}
