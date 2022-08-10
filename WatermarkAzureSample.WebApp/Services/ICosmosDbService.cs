using WatermarkAzureSample.WebApp.Models;

namespace WatermarkAzureSample.WebApp.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<WatermarkItem>> GetItemsAsync(string queryString);
    Task<WatermarkItem> GetItemAsync(string id);
    Task AddItemAsync(WatermarkItem item);
}
