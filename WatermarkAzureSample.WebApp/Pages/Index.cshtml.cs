using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WatermarkAzureSample.WebApp.Models;
using WatermarkAzureSample.WebApp.Services;
using WatermarkAzureSample.WebApp.ViewModels;

namespace WatermarkAzureSample.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly WatermarkAzureSampleOptions _options;
        private readonly ICosmosDbService _cosmosDbService;
        public IndexModel(ILogger<IndexModel> logger, IOptions<WatermarkAzureSampleOptions> options, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            _options = options.Value;
            _cosmosDbService = cosmosDbService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(WatermarkAddViewModel watermarkAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var id = Guid.NewGuid().ToString("N");

                var fileExtension = Path.GetExtension(watermarkAddViewModel.ImageFile.FileName);

                var containerClient = new BlobContainerClient(_options.Blob.ConnectionString, _options.Blob.ImageContainerName);

                var blobClient = containerClient.GetBlobClient(string.Format("{0}{1}", id, fileExtension.ToLower()));

                using (Stream stream = watermarkAddViewModel.ImageFile.OpenReadStream())
                {
                    var response = await blobClient.UploadAsync(stream);
                    var rawResponse = response.GetRawResponse();
                    if (!rawResponse.IsError)
                    {
                        var imageFileUri = blobClient.Uri.AbsoluteUri;
                        //Save watermark text and Image file uri to Azure Cosmose DB
                        await _cosmosDbService.AddItemAsync(new WatermarkItem
                        {
                            Id = id,
                            ImageBlobName = blobClient.Name,
                            ImageUri = imageFileUri,
                            Text = watermarkAddViewModel.Text,
                            WatermarkedBlobName = string.Empty,
                            WatermarkedImageUri = string.Empty,
                            Requester = Request.GetClientIPAddress()
                        });
                        return RedirectToPage("Watermarks");
                    }
                    else
                    {
                        return Content(rawResponse.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}