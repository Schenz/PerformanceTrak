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
using Moq;
using PerformanceTrakFunctions.Security;
using PerformanceTrakFunctions.Repository;

namespace PerformanceTrakFunctions.Tests
{
    [TestClass]
    public class AddUserTests
    {
        private AddUser _fixture;

        private Mock<IAccessTokenProvider> tokenProvider;
        private Mock<IUserRepository> userRepository;

        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestInitialize]
        public void Setup()
        {
            tokenProvider = new Mock<IAccessTokenProvider>();
            userRepository = new Mock<IUserRepository>();
            _fixture = new AddUser(tokenProvider.Object, userRepository.Object);
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
            tableResult.HttpStatusCode = StatusCodes.Status201Created;
            userRepository.Setup(t => t.Add(It.IsAny<UserEntity>(), It.IsAny<String>())).Returns(Task.FromResult(tableResult));

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (CreatedResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status201Created, response.StatusCode);
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
        public void TestDuplicateRecordReturnsConflictStatus()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("Conflict");
            userRepository.Setup(t => t.Add(It.IsAny<UserEntity>(), It.IsAny<String>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (ConflictObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status409Conflict, response.StatusCode);
        }

        [TestMethod]
        public void TestStorageExceptionNotDuplicateReturnsBadRequest()
        {
            var principal = AccessTokenResult.Success(new System.Security.Claims.ClaimsPrincipal());
            tokenProvider.Setup(t => t.ValidateToken(It.IsAny<HttpRequest>())).Returns(principal);

            var exception = new StorageException("");
            userRepository.Setup(t => t.Add(It.IsAny<UserEntity>(), It.IsAny<String>())).Throws(exception);

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
            userRepository.Setup(t => t.Add(It.IsAny<UserEntity>(), It.IsAny<String>())).Throws(exception);

            var request = TestFactory.CreateHttpRequest("{\"id\": \"TEST\",\"name\": \"Test User\",\"family_name\": \"User\",\"given_name\": \"Test\",\"city\": \"\",\"country\": \"\",\"postalCode\": \"\",\"state\": \"\",\"streetAddress\": \"\",\"email\": \"test.user@daomin.com\",\"isNew\": false,}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}
