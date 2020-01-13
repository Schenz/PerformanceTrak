using SendGrid;
using SendGrid.Helpers.Mail;

namespace PerformanceTrakFunctions.Repository
{
    public interface ISendGridClient
    {
         Response SendEmail(SendGridMessage msg);
    }
}