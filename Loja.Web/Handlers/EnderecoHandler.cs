using System.Net.Http.Json;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class EnderecoHandler(IHttpClientFactory httpClientFactory) : IEnderecoHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<Endereco>> ObterEnderecoPorUserId(ObterEnderecoPorUserIdRequisicao request)
        => await _httpClient.GetFromJsonAsync<Resposta<Endereco>?>($"v1/endereco/detalhes/{request.UserId}")
           ?? new Resposta<Endereco>(null, 400, "Falha ao obter o endereco");
}