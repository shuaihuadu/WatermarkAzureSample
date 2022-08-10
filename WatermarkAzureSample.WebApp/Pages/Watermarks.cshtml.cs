using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WatermarkAzureSample.WebApp.Models;
using WatermarkAzureSample.WebApp.Services;

namespace WatermarkAzureSample.WebApp.Pages;

public class WatermarksModel : PageModel
{
    private readonly ILogger<WatermarksModel> _logger;
    private readonly WatermarkAzureSampleOptions _options;
    private readonly ICosmosDbService _cosmosDbService;

    public IEnumerable<WatermarkItem> WatermarkItems { get; set; }

    public WatermarksModel(ILogger<WatermarksModel> logger, IOptions<WatermarkAzureSampleOptions> options, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _options = options.Value;
        _cosmosDbService = cosmosDbService;
    }

    public async Task OnGetAsync()
    {
        var queryString = string.Format("SELECT * FROM c WHERE c.requester=\"{0}\"", Request.GetClientIPAddress());
        WatermarkItems = await _cosmosDbService.GetItemsAsync(queryString);
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var item = await _cosmosDbService.GetItemAsync(id);
        if (item != null)
        {
            await _cosmosDbService.DeleteItemAsync(id);

            var imageContainerClient = new BlobContainerClient(_options.Blob.ConnectionString, _options.Blob.ImageContainerName);
            await imageContainerClient.DeleteBlobIfExistsAsync(item.ImageBlobName, DeleteSnapshotsOption.IncludeSnapshots);

            var watermarkContainerClient = new BlobContainerClient(_options.Blob.ConnectionString, _options.Blob.WatermarkContainerName);
            await watermarkContainerClient.DeleteBlobIfExistsAsync(item.WatermarkedBlobName, DeleteSnapshotsOption.IncludeSnapshots);
        }
        return RedirectToPage("Watermarks");
    }
}