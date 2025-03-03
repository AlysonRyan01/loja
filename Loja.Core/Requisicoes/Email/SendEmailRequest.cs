namespace Loja.Core.Requisicoes.Email;

public class SendEmailRequest
{
    public string ToName { get; set; }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string FromName { get; } = "Equipe TVS";
    public string FromEmail { get; } = "tvseletronica@tvseletronica.com.br";
}