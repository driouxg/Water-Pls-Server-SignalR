using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SignalRTest.Services
{
    public class EmailSender : IEmailSender
    {
        //private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("dtguidry@asu.edu", "Joe Smith"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            //var response = await client.SendEmailAsync(msg);

            return client.SendEmailAsync(msg);

            //if (response.StatusCode == HttpStatusCode.Accepted)
            //    _logger.LogInformation($"Successfully sent email to {email}");
            //else
            //{
            //    var bodyResult = await response.Body.ReadAsStringAsync();
            //
            //    // Deserialize the reponse
            //    //var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);
            //}
        }
    }
}
