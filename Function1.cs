
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Company
{
    public class Bookmark
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
    public static class Bookmarks
    {
        [FunctionName("Bookmarks")]
        public static async Task<IActionResult> Run(
          [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            "post",
            Route = null)
          ] HttpRequest req,
          [CosmosDB("Bookmarks", "Bookmarks", ConnectionStringSetting = "CosmosDB")]IEnumerable<Bookmark> bookmarks, ILogger log

            )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }


        [FunctionName("GetBookmark")]
        public static IActionResult GetBookmark(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
           [CosmosDB("Bookmarks", "Bookmarks", ConnectionStringSetting = "CosmosDB", Id = "id", PartitionKey = "{Query.id}")]Bookmark bookmark, ILogger log)
         
        {
            try
            {

                return (ActionResult)new OkObjectResult(bookmark);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}