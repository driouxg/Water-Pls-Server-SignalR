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
using SignalRTest.Domain.Dto;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SignalRTest.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public async Task<SendEmailResponse> Execute(string apiKey, string subject, string message, string email)
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
            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg);

            //return client.SendEmailAsync(msg);

            if (response.StatusCode == HttpStatusCode.Accepted)
                _logger.LogInformation($"Successfully sent email to {email}");
            try
            {
                var bodyResult = await response.Body.ReadAsStringAsync();
            
                // Deserialize the reponse
                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                _logger.LogInformation($"Response from sendgrid: {sendGridResponse.ToString()}");

                // Add any errors to the response
                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
                };

                // Make sure we have at least one error
                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    // Add an unknown error
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact support." });

                // Return the response
                return errorResponse;
            }
            catch (Exception ex)
            {
                // If something unexpected happened, return message
                return new SendEmailResponse
                {
                    Errors = new List<string>(new[] { "Unknown error occurred" })
                };
            }
        }
    }
}
