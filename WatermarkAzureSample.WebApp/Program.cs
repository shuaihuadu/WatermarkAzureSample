using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using WatermarkAzureSample.WebApp;
using WatermarkAzureSample.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<WatermarkAzureSampleOptions>(builder.Configuration.GetSection(WatermarkAzureSampleOptions.WatermarkAzureSample));

builder.Services.AddSingleton<ICosmosDbService>(_ =>
{
    //���������ļ�
    var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<WatermarkAzureSampleOptions>>().Value;
    //ʵ����CosmosClient
    var cosmosClient = new CosmosClient(options.CosmosDb.Account, options.CosmosDb.Key);
    //�������ݿ������
    var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(options.CosmosDb.DatabaseName).GetAwaiter().GetResult();
    var containerResposne = databaseResponse.Database.CreateContainerIfNotExistsAsync(options.CosmosDb.ContainerName, "/id").GetAwaiter().GetResult();
    //ʵ�����Զ����CosmosDbService
    var cosmosDbService = new CosmosDbService(cosmosClient, options.CosmosDb.DatabaseName, options.CosmosDb.ContainerName);
    return cosmosDbService;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
