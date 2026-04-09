using SendGrid;
using SendGrid.Helpers.Mail;

namespace acaigalatico.Web.Services;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"] ?? string.Empty;
        _fromEmail = configuration["SendGrid:FromEmail"] ?? string.Empty;
        _fromName = configuration["SendGrid:FromName"] ?? "Açaí Galáctico";
    }

    public async Task<bool> EnviarEmail(string nome, string email, string telefone, string assunto, string mensagem)
    {
        var client = new SendGridClient(_apiKey);

        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress("liuliuvks@gmail.com");

        var subject = assunto;

        var body = $@"
        Nome: {nome}
        Email: {email}
        Telefone: {telefone}

        Mensagem:
        {mensagem}
        ";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }
}
