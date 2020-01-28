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
    public class UpdateUserTests
    {
        private UpdateUser _fixture;
        private Mock<IAccessTokenProvider> tokenProvider;
        private Mock<IUserRepository> userRepository;
        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestInitialize]
        public void Setup()
        {
            tokenProvider = new Mock<IAccessTokenProvider>();
            userRepository = new Mock<IUserRepository>();
            _fixture = new UpdateUser(tokenProvider.Object, userRepository.Object);
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

            var userEntity = new UserEntity()
            {
                Id = "TEST",
                Name = "Test User",
                FamilyName = "User",
                GivenName = "Test",
                City = "",
                Country = "",
                PostalCode = "",
                State = "",
                StreetAddress = "",
                Email = "test.user@daomin.com",
            };

            var tableResult = new TableResult();
            tableResult.Result = userEntity;
            tableResult.HttpStatusCode = StatusCodes.Status200OK;
            userRepository.Setup(t => t.Update(It.IsAny<UserEntity>())).Returns(Task.FromResult(tableResult));

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (OkObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
            var returnEntity = (UserEntity)((TableResult)response.Value).Result;
            Assert.AreEqual(userEntity.Id, returnEntity.Id);
            Assert.AreEqual(userEntity.Name, returnEntity.Name);
            Assert.AreEqual(userEntity.FamilyName, returnEntity.FamilyName);
            Assert.AreEqual(userEntity.GivenName, returnEntity.GivenName);
            Assert.AreEqual(userEntity.City, returnEntity.City);
            Assert.AreEqual(userEntity.Country, returnEntity.Country);
            Assert.AreEqual(userEntity.PostalCode, returnEntity.PostalCode);
            Assert.AreEqual(userEntity.State, returnEntity.State);
            Assert.AreEqual(userEntity.StreetAddress, returnEntity.StreetAddress);
            Assert.AreEqual(userEntity.Email, returnEntity.Email);
        }

        [TestMethod]
        public void TestStorageExceptionNotDuplicateReturnsBadRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("");
            userRepository.Setup(t => t.Update(It.IsAny<UserEntity>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void TestGoodRequestButStorageConnectionError()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new NullReferenceException("");
            userRepository.Setup(t => t.Update(It.IsAny<UserEntity>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}