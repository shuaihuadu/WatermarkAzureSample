using Newtonsoft.Json;

namespace WatermarkAzureSample.FunctionApp.Models;

public class WatermarkItem
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "text")]
    public string Text { get; set; }

    [JsonProperty(PropertyName = "imageBlobName")]
    public string ImageBlobName { get; set; }

    [JsonProperty(PropertyName = "imageUri")]
    public string ImageUri { get; set; }

    [JsonProperty(PropertyName = "watermarkedBlobName")]
    public string WatermarkedBlobName { get; set; }

    [JsonProperty(PropertyName = "watermarkedImageUri ")]
    public string WatermarkedImageUri { get; set; }

    [JsonProperty(PropertyName = "requester")]
    public string Requester { get; set; }
}