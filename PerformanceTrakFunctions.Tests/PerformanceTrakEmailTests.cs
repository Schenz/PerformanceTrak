using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PerformanceTrakFunctions.Tests
{
    [TestClass]
    public class PerformanceTrakEmailTests
    {
        private readonly ILogger testLogger = TestFactory.CreateLogger();

        [TestMethod]
        public void TestBadRequest()
        {
            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("");
                var response = (BadRequestObjectResult)await ContactEmail.Run(request, testLogger);
                Assert.AreEqual("Error While Sending Email", response.Value);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("{\"Subject\":\"Please Help\",\"Email\":\"brandon.schenz@kroger.com\",\"Phone\":\"513.204.9321\",\"FullName\":\"Brandon Schenz\",\"Message\":\"I need more info RIGHT away!\"}");
                var response = (NoContentResult)await ContactEmail.Run(request, testLogger);
                Assert.AreEqual(StatusCodes.Status204NoContent, response.StatusCode);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequestButSendGridError()
        {
            ClearSendGridEnvironmentVariables();

            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("{\"Subject\":\"Please Help\",\"Email\":\"brandon.schenz@kroger.com\",\"Phone\":\"513.204.9321\",\"FullName\":\"Brandon Schenz\",\"Message\":\"I need more info RIGHT away!\"}");
                var response = (BadRequestObjectResult)await ContactEmail.Run(request, testLogger);
                Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
            }).GetAwaiter()
            .GetResult();
        }

        private void ClearSendGridEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("SendGridApiKey", null);
        }
    }
}
