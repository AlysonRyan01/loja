using System.Net.Http.Json;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Pedidos;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class PedidoHandler(IHttpClientFactory httpClientFactory) : IPedidoHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<Resposta<Pedido?>> CancelarPedidoAsync(PedidoCanceladoRequisicao request)
    {
        var result = await _httpClient.PostAsJsonAsync($"v1/pedido/cancelar/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Resposta<Pedido?>>()
            ?? new Resposta<Pedido?>(null, 400, "Falha ao cancelar o pedido.");
    }

    public async Task<Resposta<Pedido?>> CriarPedidoAsync(CriarPedidoRequisicao request)
    {
        var result = await _httpClient.PostAsJsonAsync($"v1/pedido/create", request);
        return await result.Content.ReadFromJsonAsync<Resposta<Pedido?>>()
               ?? new Resposta<Pedido?>(null, 400, "Falha ao criar o pedido.");
    }

    public async Task<Resposta<Pedido?>> PagarPedidoAsync(PagarPedidoRequisicao request)
    {
        var result = await _httpClient.PostAsJsonAsync($"v1/pedido/pagar/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Resposta<Pedido?>>()
               ?? new Resposta<Pedido?>(null, 400, "Falha ao pagar o pedido.");
    }

    public async Task<Resposta<Pedido?>> ReembolsarPedidoAsync(ReembolsarPedidoRequisicao request)
    {
        var result = await _httpClient.PostAsJsonAsync($"v1/pedido/reembolsar/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Resposta<Pedido?>>()
               ?? new Resposta<Pedido?>(null, 400, "Falha ao estornar o pedido.");
    }

    public async Task<Resposta<List<Pedido>?>> ObterTodosOsPedidosAsync(ObterTodosOsPedidoRequisicao request)
        => await _httpClient.GetFromJsonAsync<Resposta<List<Pedido>?>>("v1/pedido/obter")
           ?? new Resposta<List<Pedido>?>(null, 400, "Não foi possível obter os pedidos");
    

    public async Task<Resposta<Pedido?>> ObterPedidoPeloNumeroAsync(ObterPedidoPeloNumeroRequisicao request)
        => await _httpClient.GetFromJsonAsync<Resposta<Pedido?>>($"v1/pedido/{request.Numero}")
           ?? new Resposta<Pedido?>(null, 400, "Não foi possível obter o pedido");
}