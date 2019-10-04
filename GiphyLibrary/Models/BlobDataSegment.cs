using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;

namespace GiphyLibrary.Models
{
    public class BlobDataSegment<T>
    {
        public List<T> Data { get; set; }
        public BlobContinuationToken Token { get; set; }
    }
}
