using Newtonsoft.Json;

namespace WatermarkAzureSample.WebApp.Models;

public class WatermarkItem
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "text")]
    public string Text { get; set; }

    [JsonProperty(PropertyName = "imageFileUri")]
    public string ImageFileUri { get; set; }

    [JsonProperty(PropertyName = "watermarkedImageFileUri")]
    public string WatermarkedImageFileUri { get; set; }

    [JsonProperty(PropertyName = "requester")]
    public string Requester { get; set; }
}
