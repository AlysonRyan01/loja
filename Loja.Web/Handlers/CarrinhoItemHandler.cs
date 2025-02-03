using System.Net.Http.Json;
using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class CarrinhoItemHandler(IHttpClientFactory httpClientFactory) : ICarrinhoItemHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

    public async Task<Resposta<CarrinhoItem>> CriarCarrinhoItemAsync(CriarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/carrinho-item/create", requisicao);
        return await result.Content.ReadFromJsonAsync<Resposta<CarrinhoItem>>()
               ?? new Resposta<CarrinhoItem>(null, 400, "Falha ao criar o produto do carrinho");
    }

    public Task<Resposta<CarrinhoItem>> AtualizarrCarrinhoItemAsync(AtualizarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public async Task<Resposta<CarrinhoItem>> RemoverCarrinhoItemAsync(RemoverCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        var response = await _httpClient.DeleteAsync($"v1/carrinho-item/delete/{requisicao.Id}");

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<Resposta<CarrinhoItem>>() 
                   ?? new Resposta<CarrinhoItem>(null, 400, "Erro desconhecido");
        
        return new Resposta<CarrinhoItem>(null, (int)response.StatusCode, "Falha ao remover o item");
    }

    public async Task<Resposta<List<CarrinhoItem>?>> ObterCarrinhoItemAsync(ClaimsPrincipal user)
    {
        return await _httpClient.GetFromJsonAsync<Resposta<List<CarrinhoItem>?>>($"v1/carrinho-item/list")
               ?? new Resposta<List<CarrinhoItem>?>(null, 400, "Falha ao obter os produtos");
    }
}