using Loja.Core.Requisicoes.Email;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface IEmailHandler
{
    Task<Resposta<string>> SendEmailAsync(SendEmailRequest request);
}