using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WatermarkAzureSample.WebApp.ViewModels;

namespace WatermarkAzureSample.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(WatermarkModel watermark)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var connectionString = "YOUR STORAGE ACCOUNT CONNECTION STRING";
            var imageContainerName = "YOUR CONTAINER NAME";

            try
            {
                var containerClient = new BlobContainerClient(connectionString, imageContainerName);
                var blobClient = containerClient.GetBlobClient(watermark.ImageFile.FileName);

                using (Stream stream = watermark.ImageFile.OpenReadStream())
                {
                    var response = blobClient.Upload(stream);
                    var rawResponse = response.GetRawResponse();
                    if (!rawResponse.IsError)
                    {

                        return Content(blobClient.Uri.AbsoluteUri);
                        //todo Save Text and AbsoluteUri to Azure Cosmose DB
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