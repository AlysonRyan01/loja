using System.Net;
using System.Net.Mail;
using Loja.Core;
using Loja.Core.Requisicoes.Email;

namespace Loja.Api.Services;

public class EmailService
{
    public bool SendEmail(SendEmailRequest request)
    {
        var smtpClient = new SmtpClient(ApiConfiguration.Smtp.Host, ApiConfiguration.Smtp.Port);
        
        smtpClient.Credentials = new NetworkCredential(ApiConfiguration.Smtp.UserName, ApiConfiguration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        
        var mail = new MailMessage();
        
        mail.From = new MailAddress(request.FromEmail, request.FromName);
        mail.To.Add(new MailAddress(request.ToEmail, request.ToName));
        mail.Subject = request.Subject;
        mail.Body = request.Body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
        
    }
}