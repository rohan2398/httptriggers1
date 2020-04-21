using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Microsoft.Azure.Management.CosmosDB.Fluent;
using System.Configuration;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;


namespace EmployeeFunction
{
    public class Employee
    {
        // EMPLOYEE DETAILS
        [FunctionName("Employee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
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

        // GET EMPLOYEE
        [FunctionName("GetEmployees")]
        public static IActionResult GetEmployees([HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "GetEmployees")]HttpRequest req, ILogger log,
            [CosmosDB(databaseName: "Employee", collectionName: "Employee", ConnectionStringSetting = "CosmosDBConnection", SqlQuery = "Select * from Employee")]IEnumerable<EmployeeEntity> EmpEntity)
        {
            try
            {

                if (EmpEntity.Count() >= 1)
                {
                    log.LogInformation("Getting Employees");
                    return new OkObjectResult(EmpEntity);
                }
                //var resp = new HttpResponseMessage { Content = new StringContent("Data Not Found", System.Text.Encoding.UTF8, "application/json") };
                //return new NotFoundObjectResult(resp);
                string str = "{ 'EmployeeEntity': { 'Success': 'True','Data': 'null','Message':'No Data Found'} }";
                dynamic json = JsonConvert.DeserializeObject(str);
                return new OkObjectResult(json);
            }
            catch
            {
                string str = "{ 'EmployeeEntity': { 'Success': 'False','Data': 'null','Message':'No Data Found'} }";
                dynamic json = JsonConvert.DeserializeObject(str);
                return new OkObjectResult(json);
            }
        }

        // CREATE EMPLOYEE
        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CreateEmployee")]HttpRequest req,
        [CosmosDB(
        databaseName: "Employee",
        collectionName: "Employee",
            PartitionKey = "/location",
        ConnectionStringSetting = "CosmosDBConnection")]
        IAsyncCollector<object> todos, ILogger log)
        {
            try
            {
                log.LogInformation("Creating a new Employee list item");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<EmployeeEntity>(requestBody);
                if (input.employeeName == "")
                {
                    string str = "{  'Success': 'false','Data': 'null','Message':'Employee Name not found' }";
                    dynamic json = JsonConvert.DeserializeObject(str);
                    return new OkObjectResult(json);
                }
                 else if (input.employeeSalary == "")
                {
                    string str = "{ 'EmployeeEntity': { 'Success': 'false','Data': 'null','Message':'Employee Salary not found'} }";
                    dynamic json = JsonConvert.DeserializeObject(str);
                    return new OkObjectResult(json);
                }
               else if (input.location == "")
                {
                    string str = "{  'Success': 'false','Data': 'null','Message':' Location not found' }";
                    dynamic json = JsonConvert.DeserializeObject(str);
                    return new OkObjectResult(json);
                }
                else if (input.address == "")
                {
                    string str = "{  'Success': 'false','Data': 'null','Message':' Address not found' }";
                    dynamic json = JsonConvert.DeserializeObject(str);
                    return new OkObjectResult(json);
                }
                var Emp = new EmployeeEntity() { employeeName = input.employeeName, employeeSalary = input.employeeSalary, location = input.location, address = input.address, pincode = input.pincode };

                //the object we need to add has to have a lower case id property or we'll
                // end up with a cosmosdb document with two properties - id (autogenerated) and Id
                await todos.AddAsync(new { id = Emp.employeeId, Emp.employeeName, Emp.employeeSalary, Emp.location, Emp.address, Emp.pincode});
                return new OkObjectResult(Emp);
            } 
            catch(Exception ex)
              {
                string str = "{ 'EmployeeEntity': { 'Success': 'False','Data': 'null','Message':'Not Created'} }";
                dynamic json = JsonConvert.DeserializeObject(str);
                return new OkObjectResult(json);
            }
        }

        // DELETE EMPLOYEE
        [FunctionName("DeleteEmployee")]
        public static async Task<IActionResult> DeleteEmployee(
      [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "{location}/DeleteEmployee/{id}")]HttpRequest req,
       [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
       ILogger log, string id ,string location)
        {
            try
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Employee", "Employee");

                var document = client.CreateDocumentQuery(collectionUri,GetFeedOptions(location)).Where(t => t.Id == id)
                .AsEnumerable().FirstOrDefault(); 
                //var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                //.AsEnumerable().FirstOrDefault();
                if (document == null)
                {
                    return new NotFoundResult();
                }
                await client.DeleteDocumentAsync(document.SelfLink,new RequestOptions { PartitionKey = new PartitionKey(location)});
                return new OkResult();
            }
            catch (Exception ex)
            {
                return null;
            } 
        }

        // UPDATE EMPLOYEE
        [FunctionName("UpdateEmployee")]
        public static async Task<IActionResult> UpdateTodo(
   [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{location}/UpdateEmployee/{id}")]HttpRequest req,
   [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")]
        DocumentClient client,
   ILogger log, string id,string location)

          {
            try
            {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updated = JsonConvert.DeserializeObject<EmployeeEntity>(requestBody);
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("Employee", "Employee");

                //var document = client.CreateDocumentQuery(collectionUri,GetFeedOptions(Id)).Where(t => t.Id == Id)
                //.AsEnumerable().FirstOrDefault();
                var document = client.CreateDocumentQuery(collectionUri,GetFeedOptions(location)).Where(t => t.Id == id)
                 .AsEnumerable().FirstOrDefault();
                if (document == null)
                {
                    return new NotFoundResult();
                }
                
                if (!string.IsNullOrEmpty(updated.employeeName))
                {
                    document.SetPropertyValue("employeeName", updated.employeeName);
                }
                else if (!string.IsNullOrEmpty(updated.employeeSalary))
                {
                    document.SetPropertyValue("employeeSalary", updated.employeeSalary);
                }

                await client.ReplaceDocumentAsync(document);

                /* var todo = new Todo()
                {
                    Id = document.GetPropertyValue<string>("id"),
                    CreatedTime = document.GetPropertyValue<DateTime>("CreatedTime"),
                    TaskDescription = document.GetPropertyValue<string>("TaskDescription"),
                    IsCompleted = document.GetPropertyValue<bool>("IsCompleted")
                };*/

                // an easier way to deserialize a Document
                Employee todo = (dynamic)document;

                return new OkObjectResult(todo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FeedOptions GetFeedOptions(string partitionKey)
        {
            if (partitionKey == null)
            {
                return new FeedOptions { EnableCrossPartitionQuery = true };
            }
            else
            {
                return new FeedOptions { PartitionKey = new PartitionKey(partitionKey) };
            }
        }

      

    }
            

    
}

      