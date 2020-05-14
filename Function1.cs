using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using Microsoft.Azure.Cosmos;

namespace Company.Function
{
    public static class CosmosDBTrigger
    {
        private static readonly string _endpointUrl = System.Environment.GetEnvironmentVariable("endpointUrl");
        private static readonly string _primaryKey = System.Environment.GetEnvironmentVariable("primaryKey");
        private static readonly string _databaseId = "database";
        private static readonly string _containerId = "collection2";
        private static CosmosClient cosmosClient = new CosmosClient(_endpointUrl, _primaryKey);


        [FunctionName("CosmosDBTrigger")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "database",
            collectionName: "collection1",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {

            var container2 = cosmosClient.GetContainer(_databaseId, _containerId);

            foreach (Document doc in input)
            {
                log.LogInformation("pushed doc into container 2");
                log.LogInformation("doc: " + doc);
                try
                {
                    await container2.CreateItemAsync<Document>(doc);
                }
                catch (Exception ex)
                {
                    log.LogInformation("Exception pushing doc into container 2: " + ex);
                }

            }
        }
    }
}