using System.Threading.Tasks;
using WatermarkAzureSample.Models;

namespace WatermarkAzureSample.Functions.Services;

public interface ICosmosDbService
{
    Task<WatermarkItem> GetItemAsync(string id);
    Task UpdateItemAsync(string id, WatermarkItem item);
}
