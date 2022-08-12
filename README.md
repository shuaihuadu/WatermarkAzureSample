# Watermark Azure Sample

In order to consolidate the knowledge of Microsoft azure, I decided to use a real world scenario, so as to better understand the relevant skills of Microsoft azure products and services.

For this purpose, I built a practical sample to use Microsoft azure related products in real scenarios and gradually complete it, for further understand the scenarios of different products and services.

This sample implemented adding text watermark to the image. 

User can add the text as watermark to the image.
User can view the watermarked image lists uploaded by themeselves.
User can delete the images and watermarked images uploaded by themeselves.

#### Note: This sample implemented in .net 6, this sample is only for the purpose of skilled use, not the best practice.

The application architecture is as below:

![arch](https://user-images.githubusercontent.com/17045801/184301691-7b179d03-9b4b-499b-8bb1-79891b59cde4.png)

As shown in the figure above, the following azure products and services will be used in this sample:

* Azure Web App  
To host the web application of watermark azure sample.

* Azure Cosmos DB  
Use the SQL API to persist the data.

* Azure Storage  
Use azure blob to store images uploaded by users and watermarked images generated.

* Azure Function  
Use azure function to process adding watermark to the image.  
In this sample, integrate with the web application by using different triggers:  
    * Azure Cosmos DB Trigger
    * Azure Blob Trigger
    * Azure Storage Queue Trigger
    * Http Trigger
    * Azure Service Bus Trigger
<br />

* Azure DevOps / Github  
Store source code and provide continuous integration and continuous delivery.
