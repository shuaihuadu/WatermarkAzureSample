namespace WatermarkAzureSample.WebApp;

public class WatermarkAzureSampleOptions
{
    public const string WatermarkAzureSample = "WatermarkAzureSample";

    public BlobOptions Blob { get; set; }
    public CosmosDbOptions CosmosDb { get; set; }
}

public class BlobOptions
{
    public string ConnectionString { get; set; }
    public string ImageContainerName { get; set; }
    public string WatermarkContainerName { get; set; }
}

public class CosmosDbOptions
{
    public string Account { get; set; }
    public string Key { get; set; }
    public string DatabaseName { get; set; }
    public string ContainerName { get; set; }
}
