using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using PerformanceTrakFunctions.Functions;
using PerformanceTrakFunctions.Models;
using PerformanceTrakFunctions.Repository;
using PerformanceTrakFunctions.Security;

namespace PerformanceTrakFunctions.Tests
{
    [TestClass]
    public class GetUserRoleTests
    {
        private GetUserRole _fixture;
        private Mock<IAccessTokenProvider> tokenProvider;
        private Mock<IUserRoleRepository> userRoleRepository;
        private readonly ILogger testLogger = TestFactory.CreateLogger();

        private readonly String userId = "TEST";

        [TestInitialize]
        public void Setup()
        {
            tokenProvider = new Mock<IAccessTokenProvider>();
            userRoleRepository = new Mock<IUserRoleRepository>();
            _fixture = new GetUserRole(tokenProvider.Object, userRoleRepository.Object);
        }

        [TestMethod]
        public void TestBadRequestNoAuthToken()
        {
            var principal = AccessTokenResult.NoToken();
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var request = TestFactory.CreateHttpRequest("");
            var response = (UnauthorizedResult)_fixture.Run(request, userId, testLogger);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status401Unauthorized);
        }

        [TestMethod]
        public void TestBadRequestWithAuthToken()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var request = TestFactory.CreateHttpRequest("");
            var response = (BadRequestObjectResult)_fixture.Run(request, "", testLogger);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status400BadRequest);
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var userRoleEntity = new UserRoleEntity()
            {
                UserId = "TEST",
                RoleName = "TestRole",
                PartitionKey = "TestRole",
                RowKey = "TEST"
            };

            var ctor = typeof(TableQuerySegment<UserRoleEntity>)
                            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                            .FirstOrDefault(c => c.GetParameters().Count() == 1);

            var mockQuerySegment = ctor.Invoke(new object[] { new List<UserRoleEntity>() { userRoleEntity } }) as TableQuerySegment<UserRoleEntity>;

            userRoleRepository.Setup(t => t.Get(It.IsAny<String>())).Returns(Task.FromResult(mockQuerySegment));

            var request = TestFactory.CreateHttpRequest("{\"userId\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (OkObjectResult)_fixture.Run(request, userId, testLogger);
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
            var returnEntityList = (TableQuerySegment<UserRoleEntity>)response.Value;
            var returnEntity = returnEntityList.FirstOrDefault();
            Assert.AreEqual(userRoleEntity.UserId, returnEntity.UserId);
            Assert.AreEqual(userRoleEntity.RoleName, returnEntity.RoleName);
        }

        [TestMethod]
        public void TestStorageExceptionReturnsBadRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("");
            userRoleRepository.Setup(t => t.Get(It.IsAny<String>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"userId\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (BadRequestObjectResult)_fixture.Run(request, userId, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void TestGoodRequestButStorageConnectionError()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new NullReferenceException("");
            userRoleRepository.Setup(t => t.Get(It.IsAny<String>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"userId\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (BadRequestObjectResult)_fixture.Run(request, userId, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}