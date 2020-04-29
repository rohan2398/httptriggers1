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

namespace EmployeeFunction
{
    public static class timerTrigger
    {
        [FunctionName("timerTrigger")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($" Timer trigger function executed at: {DateTime.Now}");

            var req = new HttpRequestMessage();
            var employeeEntity = new EmployeeEntity()
            { employeeName = "Venky", employeeSalary = "9000", location = "Delhi", address = "H-3", date = DateTime.Now };
            req.Content = new StringContent(JsonConvert.SerializeObject(employeeEntity), Encoding.UTF8, "application/json");
            req = TestHelpers.SetupHttp(req);

            log.LogInformation($" triggered successfully at: {DateTime.Now}");

            //Employee.UpdateEmployee();
        }

        //public static class TimerTrigger
        //{
        //    private static readonly object location;

        //    public static void Timertrigger()
        //    {
        //        var req = new HttpRequestMessage();
        //        var employeeEntity = new EmployeeEntity()
        //        { employeeName = "Venky", employeeSalary = "9000", location = "Delhi", address = "H-3", date = DateTime.Now };
        //        req.Content = new StringContent(JsonConvert.SerializeObject(employeeEntity), Encoding.UTF8, "application/json");
        //        req = TestHelpers.SetupHttp(req);

        //        // Employee.UpdateEmployee();


        //    }
        //}

    }
}
