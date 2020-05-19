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
using System.Linq.Expressions;
using System.Threading;

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
            input.employeeName = "TestEmployeeName";
            input.employeeSalary = "TestEmployeeSalary";
            input.location = "TestEmployeeLocation";
            input.address = "TestEmployeeName";
            input.pincode = "TestEmployeePincode";


            var req = new HttpRequestMessage();
            req.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            req = TestHelpers.SetupHttp(req);


            Expression<Func<EmployeeEntity, bool>> predicate = t => t.employeeId == "TestEmployeeId";
            var dataSource = new List<EmployeeEntity> { }.AsQueryable();
            var expected = dataSource.Where(predicate);
            var response = new FeedResponse<EmployeeEntity>(expected);

            var mockOrderDocumentQuery = TestHelpers.GetMockDocumentQuery(dataSource, response);

            var priceClient = new Mock<IDocumentClient>();
            priceClient
            .Setup(_ => _.CreateDocumentQuery<EmployeeEntity>(It.IsAny<Uri>(), It.IsAny<FeedOptions>()))
            .Returns(mockOrderDocumentQuery.Object);

            priceClient
           .Setup(_ => _.ReplaceDocumentAsync(It.IsAny<Document>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
           .Returns(Task.FromResult(new ResourceResponse<Document>()));

            var result = Employee.UpdateVisibility(req, this.loggerMock.Object, priceClient.Object);

            var okresult = result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult;
            string message = ((dynamic)okresult.Value).message as string;

            Assert.Equal(okresult.StatusCode, StatusCodes.Status200OK);
            Assert.Equal("Favourites Added Successfully!", message);



        }

    }
}
