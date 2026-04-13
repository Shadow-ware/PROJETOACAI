using System.Threading.Tasks;
using acaigalatico.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace acaigalatico.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new System.Exception("SendGrid ApiKey não configurada no appsettings.json");
            }

            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new System.Exception("SendGrid FromEmail não configurado no appsettings.json");
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Açaí Galáctico - Site");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                throw new System.Exception($"Erro ao enviar e-mail via SendGrid. Status: {response.StatusCode}. Detalhes: {responseBody}");
            }
        }
    }
}
