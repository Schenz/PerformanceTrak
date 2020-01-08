using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PerformanceTrakFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceTrakFunctions.Tests
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
                var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
                var response = (CreatedResult)await AddUser.Run(request, testLogger);
                var entity = (UserEntity)response.Value;
                Assert.AreEqual(StatusCodes.Status201Created, response.StatusCode);
                await DeleteTestRecordAsync(entity);
            }).GetAwaiter()
            .GetResult();
        }

        private async Task DeleteTestRecordAsync(UserEntity entity)
        {
            var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("Environment"));
            var environmentString = $"{environment.ToString()}";
            var tableName = $"user{(environment != Environments.PROD ? environmentString : string.Empty)}";

            var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("TableStoreConnectionString"))
                    .CreateCloudTableClient()
                    .GetTableReference(tableName);

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
