using System.Collections.Generic;

namespace GiphyLibrary.Models
{
    /// <summary>
    /// Client read model for Giphy
    /// </summary>
    public class Giphy
    {
        /// <summary>
        /// Id matching the Giphy API
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Caption text associated with this giphy
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Unaltered image url
        /// </summary>
        public string OriginalUrl { get; set; }
        /// <summary>
        /// List of tags associeted with this giphy if pulled from storage
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
