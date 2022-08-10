创建示例所需资源：
1.创建资源组
az group create --name WatermarkSample --location eastasia

2.创建存储账户
az storage account create --name watermarksample --location eastasia --resource-group WatermarkSample --sku Standard_LRS --kind StorageV2 --access-tier hot

3.创建Blob存储容器

创建图像容器
az storage container create --name images --account-name watermarksample --account-key YOUR_ACCOOUNT_KEY

创建水印容器
az storage container create --name watermark --account-name watermarksample --account-key YOUR_ACCOOUNT_KEY

4.创建应用服务计划
az appservice plan create --name watermarkServicePlan --resource-group WatermarkSample --sku Free

5.创建Web应用

az webapp create --name WatermarkSampleApp --resource-group WatermarkSample --plan watermarkServicePlan

6.部署水印应用程序

1.从Github直接部署
az webapp deployment source config --name WatermarkSampleApp --resource-group WatermarkSample --branch master --manual-integration --repo-url https://github.com/shuaihuadu/WatermarkSample
2.使用VS进行部署

上传文件
使用Azure C# SDK进行文件上传
dotnet add  package Azure.Storage.Blobs

使用Azure Cosmos DB存储数据：

dotnet add package Microsoft.Azure.Cosmos
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Cosmos

我们使用EF Core进行Azure Cosmos DB的持久化操作。