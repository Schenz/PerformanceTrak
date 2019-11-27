using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PerformanceTrakEmail.Test
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
                var response = (BadRequestObjectResult) await PerformanceTrakEmail.Run(request, testLogger);
                Assert.AreEqual("Error While Sending Email", response.Value);
            }).GetAwaiter()
            .GetResult();
        }

        [TestMethod]
        public void TestGoodRequest()
        {
            SetEnvironmentVariables();
            
            Task.Run(async () =>
            {
                var request = TestFactory.CreateHttpRequest("{\"Subject\":\"Please Help\",\"Email\":\"brandon.schenz@kroger.com\",\"Phone\":\"513.204.9321\",\"FullName\":\"Brandon Schenz\",\"Message\":\"I need more info RIGHT away!\"}");
                var response = (NoContentResult) await PerformanceTrakEmail.Run(request, testLogger);
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
                var response = (BadRequestObjectResult) await PerformanceTrakEmail.Run(request, testLogger);
                Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
            }).GetAwaiter()
            .GetResult();
        }

        private void ClearSendGridEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("SendGridApiKey", null);
        }

        public void SetEnvironmentVariables()
        {
            using (var launchSettingsFile = File.OpenText("Properties/launchSettings.json"))
            {
                // Load environment variables from json configuration file
                JsonTextReader jsonReader = new JsonTextReader(reader: launchSettingsFile);
                JObject jsonObject = JObject.Load(reader: jsonReader);

                // Get variables from config file into List
                List<JProperty> envVariables = jsonObject
                    .GetValue("profiles")
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(property => property.Name == "environmentVariables")
                    .SelectMany(property => property.Value.Children<JProperty>())
                    .ToList();

                // Manually set each variable read from config file
                envVariables.ForEach(variable =>
                {
                    Environment.SetEnvironmentVariable(
                        variable: variable.Name,
                        value: variable.Value.ToString());
                });
            }
        }
    }
}