using System.Threading.Tasks;
using WatermarkAzureSample.FunctionApp.Models;

namespace WatermarkAzureSample.FunctionApp.Services;

public interface ICosmosDbService
{
    Task<WatermarkItem> GetItemAsync(string id);
    Task UpdateItemAsync(string id, WatermarkItem item);
}
