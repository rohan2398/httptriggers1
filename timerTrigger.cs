using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Management.CosmosDB.Fluent;
using System.Configuration;
using ECP.Master.tests;
using MySql.Data.MySqlClient.Memcached;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.IO;
using Castle.Core.Internal;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;


namespace EmployeeFunction
{
    public static class timerTrigger
    {
        //[FunctionName("timerTrigger")]
        //public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($" Timer trigger function executed at: {DateTime.Now}");

        //    var req = new HttpRequestMessage();
        //    var employeeEntity = new EmployeeEntity()
        //    { employeeName = "Venky", employeeSalary = "9000", location = "Delhi", address = "H-3", date = DateTime.Now };
        //    req.Content = new StringContent(JsonConvert.SerializeObject(employeeEntity));
        //    req = TestHelpers.SetupHttp(req);

        //    log.LogInformation($" triggered successfully at: {DateTime.Now}");

        //    Employee.UpdateEmployee(req, DocumentClient client, ILogger log, string id, string location));
        //}

        //public static async Task<IActionResult> UpdateEmployee(
        //   [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{location}/UpdateEmployee/{id}")]HttpRequest req,
        //    [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")]
        //    DocumentClient client,
        //      ILogger log, string id, string location)



        [FunctionName("TimerTrigger")]
        public static void Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
              // [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{location}/UpdateEmployee/{id}")]
              HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            //[TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "%TwilioFromNumber%")]
            out CreateMessageOptions messageToSend,
          ILogger log)

        {
            try
            {

                //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
               // var updated = JsonConvert.DeserializeObject<EmployeeEntity>(requestBody);
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Employee", "Employee");
                var options = new FeedOptions { EnableCrossPartitionQuery = true };
                DateTime startOfToday = DateTime.Today;
                DateTime endOfToday = startOfToday.AddDays(1).AddTicks(-1);

                decimal totalValueOfTodaysOrders =
                    client.CreateDocumentQuery<order>(collectionUri, options)
                          .Where(order => order.orderDate >= startOfToday && order.orderDate <= endOfToday)
                          .Sum(order => order.orderTotal);
               // var document = client.CreateDocumentQuery(collectionUri, GetFeedOptions(location)).Where(t => t.Id == id)
               //.AsEnumerable().FirstOrDefault();


                var messageText = $"Total sales for today: {totalValueOfTodaysOrders}";

                log.LogInformation(messageText);

                var managersMobileNumber = new PhoneNumber(Environment.GetEnvironmentVariable("ManagersMobileNumber"));

                var mobileMessage = new CreateMessageOptions(managersMobileNumber)
                {
                    Body = messageText
                };

                messageToSend = mobileMessage;
            }

            catch(Exception ex)
            {
                messageToSend = null;
            }
        }
    }
 
}

