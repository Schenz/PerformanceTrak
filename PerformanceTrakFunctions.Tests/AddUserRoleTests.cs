using System;
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
    public class AddUserRoleTests
    {
        private AddUserRole _fixture;

        private Mock<IAccessTokenProvider> tokenProvider;
        private Mock<IUserRoleRepository> userRoleRepository;

        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestInitialize]
        public void Setup()
        {
            tokenProvider = new Mock<IAccessTokenProvider>();
            userRoleRepository = new Mock<IUserRoleRepository>();
            _fixture = new AddUserRole(tokenProvider.Object, userRoleRepository.Object);
        }

        [TestMethod]
        public void TestBadRequestNoAuthToken()
        {
            var principal = AccessTokenResult.NoToken();
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var request = TestFactory.CreateHttpRequest("");
            var response = (UnauthorizedResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status401Unauthorized);
        }

        [TestMethod]
        public void TestBadRequestWithAuthToken()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var request = TestFactory.CreateHttpRequest("");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status400BadRequest);
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var userEntity = new UserRoleEntity()
            {
                Id = "TEST",
                RoleName = "TestRole",
            };

            var tableResult = new TableResult();
            tableResult.Result = userEntity;
            tableResult.HttpStatusCode = StatusCodes.Status201Created;
            userRoleRepository.Setup(t => t.Add(It.IsAny<UserRoleEntity>())).Returns(Task.FromResult(tableResult));

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (CreatedResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status201Created, response.StatusCode);
            var returnEntity = (UserRoleEntity)((TableResult)response.Value).Result;
            Assert.AreEqual(userEntity.Id, returnEntity.Id);
            Assert.AreEqual(userEntity.RoleName, returnEntity.RoleName);
        }

        [TestMethod]
        public void TestDuplicateRecordReturnsConflictStatus()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("Conflict");
            userRoleRepository.Setup(t => t.Add(It.IsAny<UserRoleEntity>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (ConflictObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status409Conflict, response.StatusCode);
        }

        [TestMethod]
        public void TestStorageExceptionNotDuplicateReturnsBadRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("");
            userRoleRepository.Setup(t => t.Add(It.IsAny<UserRoleEntity>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void TestGoodRequestButStorageConnectionError()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new NullReferenceException("");
            userRoleRepository.Setup(t => t.Add(It.IsAny<UserRoleEntity>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"roleName\": \"TestRole\",}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}