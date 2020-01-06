using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Text;

namespace PerformanceTrakEmail
{
    public static class PerformanceTrakEmail
    {
        [FunctionName("PerformanceTrackEmail")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ContactEmail")] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<EmailObject>(requestBody);
                var msg = MailHelper.CreateSingleEmail(new EmailAddress(data.Email, data.FullName), new EmailAddress("bschenz@gmail.com", "Brandon Schenz"), data.Subject, BuildPlainTextContent(data), BuildHtmlContent(data));
                var response = await new SendGridClient(Environment.GetEnvironmentVariable("SendGridApiKey")).SendEmailAsync(msg);

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    return (ActionResult)new NoContentResult();
                }
                else
                {
                    log.LogWarning("SendGrid Failed: StatusCode: {0} Body: {1} Headers: {2}", response.StatusCode, response.Body, response.Headers);
                    return new BadRequestObjectResult(response.Body);
                }
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, "Error While Sending Email");
                return new BadRequestObjectResult("Error While Sending Email");
            }
        }

        private static string BuildHtmlContent(EmailObject data)
        {
            var builder = new StringBuilder();
            builder.Append("<p>Email from: <br />");
            builder.Append($"{data.FullName}<br />");
            builder.Append($"{data.Email}<br />");
            builder.Append($"{data.Phone}</p>");
            builder.Append("<p>Message: <br /><hr />");
            builder.Append($"{data.Message}</p>");

            return builder.ToString();
        }

        private static string BuildPlainTextContent(EmailObject data)
        {
            var builder = new StringBuilder();
            builder.Append("Email from: \r\n");
            builder.Append($"{data.FullName}\r\n");
            builder.Append($"{data.Email}\r\n");
            builder.Append($"{data.Phone}\r\n");
            builder.Append("Message: \r\n");
            builder.Append(data.Message);

            return builder.ToString();
        }
    }
}