using Microsoft.Azure.Cosmos;
using System.Net;
using WatermarkAzureSample.WebApp.Models;

namespace WatermarkAzureSample.WebApp.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _cosmosDbContainer;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
    {
        _cosmosDbContainer = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync(WatermarkItem item)
    {
        await _cosmosDbContainer.CreateItemAsync(item, new PartitionKey(item.Id.ToString()));
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

    public async Task<IEnumerable<WatermarkItem>> GetItemsAsync(string queryString)
    {
        var query = _cosmosDbContainer.GetItemQueryIterator<WatermarkItem>(new QueryDefinition(queryString));

        var results = new List<WatermarkItem>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task DeleteItemAsync(string id)
    {
        await _cosmosDbContainer.DeleteItemAsync<WatermarkItem>(id, new PartitionKey(id));
    }
}
