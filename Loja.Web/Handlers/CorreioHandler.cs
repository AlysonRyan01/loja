using System.Net.Http.Json;
using System.Xml.Linq;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Correios;
using Loja.Core.Respostas;
using Loja.Core.Respostas.MelhorEnvio;

namespace Dima.Web.Handlers;

public class CorreioHandler(IHttpClientFactory httpClientFactory) : ICorreioHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

    public async Task<Resposta<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/correios/frete", request);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Resposta<List<CalculoFreteResponse>>>();
            return result ?? new Resposta<List<CalculoFreteResponse>>(null, 500, "Erro ao obter o valor do frete");
        }
        
        return new Resposta<List<CalculoFreteResponse>>(null, 500, $"Erro ao calcular o frete. Status: {response.StatusCode}");
    }
}