using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AddUser.Test
{
    [TestClass]
    public class AddUserTests
    {
        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestMethod]
        public void TestBadRequest()
        {
            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("");
                var response = (BadRequestObjectResult)await AddUser.Run(request, testLogger);
                Assert.AreEqual("User not passed", response.Value);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("{\"id\": \"4\",\"name\": \"Brandon Schenz\",\"salaray\": 987321}");
                var response = (CreatedResult)await AddUser.Run(request, testLogger);
                var entity = (EmployeeSessionEntity)response.Value;
                Assert.AreEqual(StatusCodes.Status201Created, response.StatusCode);
                await DeleteTestRecordAsync(entity);
            }).GetAwaiter()
            .GetResult();
        }

        private async Task DeleteTestRecordAsync(EmployeeSessionEntity entity)
        {
            var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("TableStoreConnectionString"))
                    .CreateCloudTableClient()
                    .GetTableReference("employee");

            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Delete(entity));
        }

        [TestMethod]
        public void TestGoodRequestButStorageConnectionError()
        {
            ClearEnvironmentVariable();

            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("{\"id\": \"4\",\"name\": \"Brandon Schenz\",\"salaray\": 987321}");
                var response = (BadRequestObjectResult)await AddUser.Run(request, testLogger);
                Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
            }).GetAwaiter()
            .GetResult();
        }

        private void ClearEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("TableStoreConnectionString", null);
        }
    }
}
