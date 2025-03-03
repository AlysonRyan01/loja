using System.Net.Http.Json;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Email;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class EmailHandler(IHttpClientFactory httpClientFactory) : IEmailHandler
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<string>> SendEmailAsync(SendEmailRequest request)
    {
        var result = await httpClient.PostAsJsonAsync("v1/email/send", request);
        return await result.Content.ReadFromJsonAsync<Resposta<string>>()
               ?? new Resposta<string>("Erro ao enviar o email", 400, "Erro ao enviar o email");
    }
}