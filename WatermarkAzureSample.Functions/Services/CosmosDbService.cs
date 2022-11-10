using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WatermarkAzureSample.Models;

namespace WatermarkAzureSample.Functions.Services;

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

    public async Task<IList<WatermarkItem>> GetByImageBlobName(string imageBlobName)
    {
        var queryable = _cosmosDbContainer.GetItemLinqQueryable<WatermarkItem>();
        var matches = queryable.Where(wi => wi.ImageBlobName == imageBlobName);

        using FeedIterator<WatermarkItem> feed = matches.ToFeedIterator();

        var result = Enumerable.Empty<WatermarkItem>().ToList();

        while (feed.HasMoreResults)
        {
            FeedResponse<WatermarkItem> response = await feed.ReadNextAsync();

            foreach (var item in response)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public async Task UpdateItemAsync(string id, WatermarkItem item)
    {
        await _cosmosDbContainer.UpsertItemAsync(item, new PartitionKey(id));
    }
}
