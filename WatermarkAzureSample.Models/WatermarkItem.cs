﻿using Newtonsoft.Json;

namespace WatermarkAzureSample.Models;

public class WatermarkItem
{
    public const string STATUS_UPLOAD = "upload";
    public const string STATUS_OK = "ok";

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

    [JsonProperty(PropertyName = "watermarkedImageUri")]
    public string WatermarkedImageUri { get; set; }
    [JsonProperty(PropertyName = "processFunction")]
    public string ProcessFunction { get; set; }
    /// <summary>
    /// upload, ok
    /// </summary>
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    [JsonProperty(PropertyName = "requester")]
    public string Requester { get; set; }
}