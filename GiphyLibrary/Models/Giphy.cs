namespace GiphyLibrary.Models
{
    public class Giphy
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string DownsizedUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string[] Tags { get; set; }
    }
}
