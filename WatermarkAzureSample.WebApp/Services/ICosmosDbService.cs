using WatermarkAzureSample.Models;

namespace WatermarkAzureSample.WebApp.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<WatermarkItem>> GetItemsAsync(string queryString);
    Task<WatermarkItem> GetItemAsync(string id);
    Task AddItemAsync(WatermarkItem item);
    Task DeleteItemAsync(string id);
}
