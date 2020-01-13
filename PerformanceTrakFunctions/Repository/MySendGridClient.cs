using System;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PerformanceTrakFunctions.Repository
{
    public class MySendGridClient : ISendGridClient
    {
        public Response SendEmail(SendGridMessage msg) => new SendGridClient(Environment.GetEnvironmentVariable("SENDGRIDAPIKEY")).SendEmailAsync(msg).Result;
    }
}