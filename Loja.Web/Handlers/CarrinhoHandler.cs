using System.Net.Http.Json;
using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class CarrinhoHandler(IHttpClientFactory httpClientFactory) : ICarrinhoHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<Carrinho>> ObterCarrinhoPorUserAsync(ClaimsPrincipal user)
    {
        return await _httpClient.GetFromJsonAsync<Resposta<Carrinho>?>($"v1/carrinho/detalhes")
               ?? new Resposta<Carrinho>(null, 400, "Falha ao obter o carrinho");
    }
}