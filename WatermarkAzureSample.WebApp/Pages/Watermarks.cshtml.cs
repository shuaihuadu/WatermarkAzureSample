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

    public void OnGet()
    {
        var queryString = string.Format("SELECT * FROM c WHERE c.requester=\"{0}\"", Request.GetClientIPAddress());
        WatermarkItems = _cosmosDbService.GetItemsAsync(queryString).GetAwaiter().GetResult();
    }
}