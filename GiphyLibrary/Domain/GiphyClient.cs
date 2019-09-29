using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using GiphyDotNet.Model.Results;
using System;
using System.Threading.Tasks;

namespace GiphyLibrary.Domain
{
    public interface IGiphyClient
    {
        Task<GiphySearchResult> SearchGiphy(string searchString, int offset);
    }

    public class GiphyClient : IGiphyClient
    {
        private readonly GiphyClientConfiguration configuration;
        private readonly Lazy<Giphy> _giphy;

        public GiphyClient(GiphyClientConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrEmpty(configuration.AuthKey))
            {
                _giphy = new Lazy<Giphy>(() => new Giphy());
            }
            else
            {
                _giphy = new Lazy<Giphy>(() => new Giphy(configuration.AuthKey));
            }
        }

        public async Task<GiphySearchResult> SearchGiphy(string searchString, int offset)
        {
            var searchParameters = new SearchParameter
            {
                Rating = configuration.Rating,
                Limit = configuration.Limit,
                Offset = offset,
                Query = searchString
            };

            return await _giphy.Value.GifSearch(searchParameters);
        }

        public async Task<GiphyIdResult> GetGiphy(string id)
        {
            return await _giphy.Value.GetGifById(id);
        }

        public async Task<GiphyIdsResult> GetManyGiphy(string[] ids)
        {
            return await _giphy.Value.GetGifsByIds(ids);
        }
    }
}
