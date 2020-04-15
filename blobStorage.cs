using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting.Internal;
using System.Web.Hosting;

namespace blobStorage
{

    public class blobStorage
    {
        //[FunctionName("Function1")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    return name != null
        //        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        //        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        //}


        public class ImageDomain
        {
            private static readonly string StorageConnectionString = System.Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            public static async Task<string> UploadImage(dynamic input, string id, ILogger log)
            {
                string output = GetImageName(id);

                string data = input?.data;
                var bytes = Convert.FromBase64String(data);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    CloudStorageAccount storageAccount;
                    if (CloudStorageAccount.TryParse(StorageConnectionString, out storageAccount))
                    {
                        // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                        CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                        string folderName = ConfigHelper.GetConfigValue(log, Constants.GlobalConfigSection, "profileImagePath");

                        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(folderName);
                        await cloudBlobContainer.CreateIfNotExistsAsync();

                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(output);
                        cloudBlockBlob.Properties.ContentType = "image/jpeg";

                        ms.Seek(0, SeekOrigin.Begin);
                        await cloudBlockBlob.UploadFromStreamAsync(ms);

                        return cloudBlockBlob.Uri.AbsoluteUri;
                    }
                }

                throw new Exception("Unable to upload the image");
            }

        }
    }
}


