using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WatermarkAzureSample.FunctionApp.Models;
using WatermarkAzureSample.FunctionApp.Services;

namespace WatermarkAzureSample.FunctionApp
{
    public static class AddWatermarkFunction
    {
        private static readonly string AzureWebJobsStorageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private static readonly string ImageBlobContainerName = Environment.GetEnvironmentVariable("WatermarkImageBlobContainerName");
        private static readonly string WatermarkedImageBlobContainerName = Environment.GetEnvironmentVariable("WatermarkedImageBlobContainerName");
        private static readonly string WatermarkSampleDbConnection = Environment.GetEnvironmentVariable("WatermarkSampleDbConnection");
        private static readonly string WatermarkSampleDatabaseName = Environment.GetEnvironmentVariable("WatermarkSampleDatabaseName");
        private static readonly string WatermarkSampleDbContainerName = Environment.GetEnvironmentVariable("WatermarkSampleDbContainerName");

        [FunctionName("AddWatermark")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "WatermarkSample",
            collectionName: "watermarkitems",
            ConnectionStringSetting = "WatermarkSampleDbConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            try
            {
                if (input != null && input.Count > 0)
                {
                    var blobServiceClient = new BlobServiceClient(AzureWebJobsStorageConnectionString);
                    var imageBlobContainerClient = blobServiceClient.GetBlobContainerClient(ImageBlobContainerName);
                    var watermarkedImageBlobContainerClient = blobServiceClient.GetBlobContainerClient(WatermarkedImageBlobContainerName);

                    foreach (var item in input)
                    {
                        //Seralize the input[0] to watermark item
                        var watermarkItem = JsonConvert.DeserializeObject<WatermarkItem>(JsonConvert.SerializeObject(item));

                        if (watermarkItem != null && !string.IsNullOrEmpty(watermarkItem.Id))
                        {
                            using (var output = new MemoryStream())
                            {

                                var imageBlobClient = imageBlobContainerClient.GetBlobClient(watermarkItem.ImageBlobName);

                                using (var stream = await imageBlobClient.OpenReadAsync())
                                {
                                    using (var image = Image.Load(stream))
                                    {
                                        var font = SystemFonts.CreateFont("Arial", image.Height, FontStyle.Bold);
                                        image.Mutate(x => x.DrawText(watermarkItem.Text, font, Color.FromRgba(255, 0, 0, 100), new PointF(0, 0)));
                                        var extension = Path.GetExtension(watermarkItem.ImageUri);
                                        var encoder = GetEncoder(extension);
                                        image.Save(output, encoder);
                                        output.Position = 0;

                                        //Upload the watermarked image to blob
                                        var watermarkedImageBlobClient = watermarkedImageBlobContainerClient.GetBlobClient(watermarkItem.ImageBlobName);
                                        var response = await watermarkedImageBlobClient.UploadAsync(output);
                                        var rawResponse = response.GetRawResponse();
                                        if (!rawResponse.IsError)
                                        {

                                            log.LogInformation(string.Format("Upload Successed, Id:{0},WatermarkedImageUri:{1}", watermarkItem.Id, watermarkItem.WatermarkedImageUri));
                                            //Save watermarked image blob name and watermarked image uri to Azure Cosmose DB
                                            watermarkItem.WatermarkedBlobName = watermarkedImageBlobClient.Name;
                                            watermarkItem.WatermarkedImageUri = watermarkedImageBlobClient.Uri.AbsoluteUri;
                                            await UpdateWatermarkItemAsync(watermarkItem.Id, watermarkItem);
                                            log.LogInformation(string.Format("Update Successed, Item:{0}", JsonConvert.SerializeObject(watermarkItem)));
                                        }
                                        else
                                        {
                                            log.LogError(string.Format("{0}-{1}", rawResponse.Status, rawResponse.ReasonPhrase));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw ex;
            }
        }

        private static async Task UpdateWatermarkItemAsync(string id, WatermarkItem item)
        {
            var cosmosClient = new CosmosClient(WatermarkSampleDbConnection);
            var cosmosDbService = new CosmosDbService(cosmosClient, WatermarkSampleDatabaseName, WatermarkSampleDbContainerName);
            await cosmosDbService.UpdateItemAsync(id, item);
        }

        private static IImageEncoder GetEncoder(string extension)
        {
            IImageEncoder encoder = null;
            extension = extension.Replace(".", "");
            var isSupported = Regex.IsMatch(extension, "png|jpg", RegexOptions.IgnoreCase);
            if (isSupported)
            {
                switch (extension.ToLower())
                {
                    case "png":
                        encoder = new PngEncoder();
                        break;
                    case "jpg":
                        encoder = new JpegEncoder();
                        break;
                    default:
                        break;
                }
            }
            return encoder;
        }
    }
}
