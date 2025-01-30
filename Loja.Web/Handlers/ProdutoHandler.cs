using System.Net.Http.Json;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class ProdutoHandler(IHttpClientFactory httpClientFactory) : IProdutoHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/produto/criar/", requisicao);
        return await result.Content.ReadFromJsonAsync<Resposta<Produto?>>()
               ?? new Resposta<Produto?>(null, 400, "Falha ao criar o produto");
    }

    public async Task<Resposta<Produto?>> AtualizarProdutoAsync(AtualizarProdutoRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public async Task<Resposta<Produto?>> RemoverProdutoAsync(RemoverProdutoRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public async Task<Resposta<Produto?>> ObterProdutoPorIdAsync(ObterProdutoPorIdRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public async Task<Resposta<List<Produto>?>> ObterTodosProdutos()
    {
        return await _httpClient.GetFromJsonAsync<Resposta<List<Produto>?>>($"v1/produto/list")
            ?? new Resposta<List<Produto>?>(null, 400, "Falha ao obter os produtos");
    }
}