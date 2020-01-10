using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PerformanceTrakFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PerformanceTrakFunctions.Functions;
using PerformanceTrakFunctions.Util;
using Moq;

namespace PerformanceTrakFunctions.Tests
{
    [TestClass]
    public class AddUserTests
    {
        private AddUser _fixture;

        private Mock<IAccessTokenProvider> tokenProvider;

        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestInitialize]
        public void Setup()
        {
            tokenProvider = new Mock<IAccessTokenProvider>();
            _fixture = new AddUser(tokenProvider.Object);
        }

        [TestMethod]
        public void TestBadRequestNoAuthToken()
        {
            Task.Run(async () =>
            {
                var principal = AccessTokenResult.NoToken();
                tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

                var request = TestFactory.CreateHttpRequest("");
                var response = (UnauthorizedResult)await _fixture.Run(request, testLogger);
                Assert.AreEqual(response.StatusCode, StatusCodes.Status401Unauthorized);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestBadRequestWithAuthToken()
        {
            Task.Run(async () =>
            {
                var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
                tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

                var request = TestFactory.CreateHttpRequest("");
                var response = (BadRequestObjectResult)await _fixture.Run(request, testLogger);
                Assert.AreEqual(response.StatusCode, StatusCodes.Status400BadRequest);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            Task.Run(async () =>
            {
                var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
                tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

                var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
                var response = (CreatedResult)await _fixture.Run(request, testLogger);
                var entity = (UserEntity)response.Value;
                Assert.AreEqual(StatusCodes.Status201Created, response.StatusCode);
                await DeleteTestRecordAsync(entity);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestDuplicateRecordReturnsConflictStatus()
        {
            Task.Run(async () =>
            {
                var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
                tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

                var request1 = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
                var request2 = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
                var response = (CreatedResult)_fixture.Run(request1, testLogger).Result;
                var response2 = (ConflictObjectResult)_fixture.Run(request2, testLogger).Result;
                var entity = (UserEntity)response.Value;
                Assert.AreEqual(StatusCodes.Status409Conflict, response2.StatusCode);
                await DeleteTestRecordAsync(entity);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequestButStorageConnectionError()
        {
            Task.Run(async () =>
            {
                ClearEnvironmentVariable();

                var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
                tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

                var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
                var response = (BadRequestObjectResult)await _fixture.Run(request, testLogger);
                Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
            }).GetAwaiter()
            .GetResult();
        }

        private async Task DeleteTestRecordAsync(UserEntity entity)
        {
            var environment = (Environments)int.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT"));
            var environmentString = $"{environment.ToString()}";
            var tableName = $"Users{(environment != Environments.PROD ? environmentString : string.Empty)}";

            var table = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("TABLESTORECONNECTIONSTRING"))
                    .CreateCloudTableClient()
                    .GetTableReference(tableName);

            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Delete(entity));
        }

        private void ClearEnvironmentVariable() => Environment.SetEnvironmentVariable("TABLESTORECONNECTIONSTRING", null);
    }
}
