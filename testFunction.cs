using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Net.Http;
using System.Linq;
using Microsoft.Azure.Management.CosmosDB.Fluent;
using System.Configuration;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Castle.Core.Internal;
using EmployeeFunction;
using ECP.Master.tests;

namespace FunctionApp2.TESTING
{
   public class testFunction
    {
        private Mock<ILogger> loggerMock;

        [Fact]
        public void TestSetup()
        {
            this.loggerMock = new Mock<ILogger>();
        }

            [Fact]
        public void CreateEmployeeList()
        {
            this.TestSetup();

            EmployeeEntity input = new EmployeeEntity();
            input.employeeId = "TestEmployeeId";

            var req = new HttpRequestMessage();
            req.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            req = TestHelpers.SetupHttp(req);


        }

    }
}
