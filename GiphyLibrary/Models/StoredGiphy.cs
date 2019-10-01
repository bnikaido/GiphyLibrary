using System.Collections.Generic;

namespace GiphyLibrary.Models
{
    public class StoredGiphy
    {
        public string Id => $"{GiphyId}_{User}";
        public string GiphyId { get; set; }
        public string User { get; set; }
        public string OriginalUrl { get; set; }
        public string DownsizedUrl { get; set; }
        public List<string> Tags { get; set; }
    }
}
