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
    [Route("[controller]")]
    public class GiphyController : ControllerBase
    {
        private readonly ILogger<GiphyController> logger;
        private readonly IGiphyClient client;

        public GiphyController(ILogger<GiphyController> logger, IGiphyClient client)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpGet("Search/{searchString}")]
        public async Task<IEnumerable<Giphy>> SearchGiphies(string searchString)
        {
            var results = await client.SearchGiphies(searchString, 0);
            return results.Data.Select(data => new Giphy { 
                Id = data.Id, 
                Caption = data.Caption,
                OriginalUrl = data.Images.Original.Url
            });
        }
    }
}
