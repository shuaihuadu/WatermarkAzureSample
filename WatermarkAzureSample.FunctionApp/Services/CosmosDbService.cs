using Microsoft.Azure.Cosmos;
using System.Net;
using System.Threading.Tasks;
using WatermarkAzureSample.FunctionApp.Models;

namespace WatermarkAzureSample.FunctionApp.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _cosmosDbContainer;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
    {
        _cosmosDbContainer = dbClient.GetContainer(databaseName, containerName);
    }
    public async Task<WatermarkItem> GetItemAsync(string id)
    {
        try
        {
            var response = await _cosmosDbContainer.ReadItemAsync<WatermarkItem>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task UpdateItemAsync(string id, WatermarkItem item)
    {
        await _cosmosDbContainer.UpsertItemAsync(item, new PartitionKey(id));
    }
}
