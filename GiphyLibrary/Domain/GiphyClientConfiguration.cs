using GiphyDotNet.Model.Parameters;

namespace GiphyLibrary.Domain
{
    public class GiphyClientConfiguration
    {
        public string AuthKey { get; set; }
        public Rating Rating { get; set; }
        public int Limit { get; set; }
    }
}