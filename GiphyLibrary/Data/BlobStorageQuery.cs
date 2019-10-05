using GiphyLibrary.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GiphyLibrary.Data
{
    public interface IBlobStorageQuery
    {
        Task<T> GetBlob<T>(string containerName, string fileName);
        Task<BlobDataSegment<T>> GetBlobsInContainer<T>(string containerName, BlobContinuationToken currentToken = null);
        Task<Uri> UploadBlob<T>(T blob, string containerName, string fileName, string contentType = "application/json");
    }

    public class BlobStorageQuery : IBlobStorageQuery
    {
        private readonly ILogger<BlobStorageQuery> logger;
        private readonly CloudBlobClient client;
        private readonly JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        private const string JsonContent = "application/json";
        private const int StringBuffer = 256;
        private const int MaxAzureBlobStorageNameLength = 62;

        private static Regex AzureBlobStorageNameRestrictions = new Regex(@"[^A-Za-z0-9]");

        public BlobStorageQuery(ILogger<BlobStorageQuery> logger, CloudBlobClient client)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<T> GetBlob<T>(string containerName, string fileName)
        {
            containerName = EncodeAzureBlobStorageName(containerName);
            fileName = EncodeAzureBlobStorageName(fileName);

            var blobUri = new Uri(client.BaseUri, $"{containerName}/{fileName}");
            return await GetBlob<T>(blobUri).ConfigureAwait(false);
        }

        public async Task<T> GetBlob<T>(Uri blobUri)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    var blob = await client.GetBlobReferenceFromServerAsync(blobUri).ConfigureAwait(false);
                    await blob.DownloadToStreamAsync(stream).ConfigureAwait(false);
                    stream.Seek(0, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(stream))
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return jsonSerializer.Deserialize<T>(jsonReader);
                    }
                }
            }
            catch (StorageException exception)
            {
                if(exception.RequestInformation.HttpStatusCode != 404)
                {
                    logger.LogWarning(exception, "Failed to get resource");
                }
                return default;
            }
        }

        public async Task<BlobDataSegment<T>> GetBlobsInContainer<T>(string containerName, BlobContinuationToken currentToken = null)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    containerName = EncodeAzureBlobStorageName(containerName);

                    var response = await client.ListBlobsSegmentedAsync(containerName, currentToken).ConfigureAwait(false);
                    var dataSegment = new BlobDataSegment<T>
                    {
                        Data = new List<T>(),
                        Token = response.ContinuationToken
                    };

                    foreach (var result in response.Results)
                    {
                        dataSegment.Data.Add(await GetBlob<T>(result.Uri));
                    }

                    return dataSegment;
                }
            }
            catch (StorageException exception)
            {
                if (exception.RequestInformation.HttpStatusCode != 404)
                {
                    logger.LogWarning(exception, "Failed to get resource");
                }
                return null;
            }
        }

        public async Task<Uri> UploadBlob<T>(T blob, string containerName, string fileName, string contentType = JsonContent)
        {
            try
            {
                containerName = EncodeAzureBlobStorageName(containerName);
                fileName = EncodeAzureBlobStorageName(fileName);

                var container = client.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = contentType;

                if (contentType.Equals(JsonContent))
                {
                    await blockBlob.UploadTextAsync(SerializeAsJson(blob));
                }
                else
                {
                    await blockBlob.UploadFromStreamAsync(blob as Stream);
                }
                return blockBlob.Uri;
            }
            catch (StorageException exception)
            {
                logger.LogWarning(exception, "Failed to upload resource");
                return null;
            }
        }

        private string EncodeAzureBlobStorageName(string name)
        {
            var validName = $"gif{AzureBlobStorageNameRestrictions.Replace(name, "")}";
            if (validName.Length > MaxAzureBlobStorageNameLength)
            {
                return validName.Substring(0, MaxAzureBlobStorageNameLength);
            }
            return validName;
        }

        private string SerializeAsJson<T>(T message)
        {
            var builder = new StringBuilder(StringBuffer);
            using (var writer = new StringWriter(builder, CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonSerializer.Serialize(jsonWriter, message, typeof(T));
                }
            }
            return builder.ToString();
        }
    }
}
