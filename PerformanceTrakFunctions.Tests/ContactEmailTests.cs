﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerformanceTrakFunctions.Functions;
using Moq;
using PerformanceTrakFunctions.Repository;
using SendGrid.Helpers.Mail;
using System.Net.Http;
using System.Net;

namespace PerformanceTrakFunctions.Tests
{
    [TestClass]
    public class ContactEmailEmailTests
    {
        private readonly ILogger testLogger = TestFactory.CreateLogger();

        private ContactEmail _fixture;

        private Mock<ISendGridClient> sendGridClient;

        [TestInitialize]
        public void Setup()
        {
            sendGridClient = new Mock<ISendGridClient>();
            _fixture = new ContactEmail(sendGridClient.Object);
        }

        [TestMethod]
        public void TestBadRequest()
        {
            var stringContent = new StringContent("");
            var tempResponse = new HttpResponseMessage();
            tempResponse.Headers.Add("deviceId", "1234");
            SendGrid.Response response1 = new SendGrid.Response(HttpStatusCode.Accepted, stringContent, tempResponse.Headers);
            sendGridClient.Setup(t => t.SendEmail(It.IsAny<SendGridMessage>())).Returns(response1);

            var request = TestFactory.CreateHttpRequest("");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual("Error While Sending Email", response.Value);
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            var stringContent = new StringContent("");
            var tempResponse = new HttpResponseMessage();
            tempResponse.Headers.Add("deviceId", "1234");
            SendGrid.Response response1 = new SendGrid.Response(HttpStatusCode.Accepted, stringContent, tempResponse.Headers);
            sendGridClient.Setup(t => t.SendEmail(It.IsAny<SendGridMessage>())).Returns(response1);

            var request = TestFactory.CreateHttpRequest("{\"Subject\":\"Please Help\",\"Email\":\"brandon.schenz@kroger.com\",\"Phone\":\"513.204.9321\",\"FullName\":\"Brandon Schenz\",\"Message\":\"I need more info RIGHT away!\"}");
            var response = (NoContentResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [TestMethod]
        public void TestGoodRequestButSendGridError()
        {
            var stringContent = new StringContent("");
            var tempResponse = new HttpResponseMessage();
            tempResponse.Headers.Add("deviceId", "1234");
            SendGrid.Response response1 = new SendGrid.Response(HttpStatusCode.BadRequest, stringContent, tempResponse.Headers);
            sendGridClient.Setup(t => t.SendEmail(It.IsAny<SendGridMessage>())).Returns(response1);

            var request = TestFactory.CreateHttpRequest("{\"Subject\":\"Please Help\",\"Email\":\"brandon.schenz@kroger.com\",\"Phone\":\"513.204.9321\",\"FullName\":\"Brandon Schenz\",\"Message\":\"I need more info RIGHT away!\"}");
            var response = (BadRequestObjectResult)_fixture.Run(request, testLogger);
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}
