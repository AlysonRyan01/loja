using System.Net.Http.Headers;
using System.Xml.Linq;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Correios;
using Loja.Core.Respostas;
using Loja.Core.Respostas.MelhorEnvio;

namespace Loja.Api.Handlers;

public class CorreioHandler : ICorreioHandler
{
    
    private readonly HttpClient _httpClient;

    public CorreioHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            ApiConfiguration.TokenMelhorEnvio);
        
        _httpClient.BaseAddress = new Uri("https://sandbox.melhorenvio.com.br/api/v2/");
    }
    
    public async Task<Resposta<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request)
    {
        try
        {
            var payload = new
            {
                from = new { postal_code = request.CepOrigem },
                to = new { postal_code = request.CepDestino },
                package = new
                {
                    weight = request.Peso,
                    height = request.Altura,
                    width = request.Largura,
                    length = request.Comprimento
                }
            };
            
            var response = await _httpClient.PostAsJsonAsync("me/shipment/calculate", payload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<CalculoFreteResponse>>();

                if (result == null || !result.Any())
                {
                    return new Resposta<List<CalculoFreteResponse>>(null, 500, "Nenhum serviço disponível.");
                }
                
                
                return new Resposta<List<CalculoFreteResponse>>(result, 200, "Frete calculado com sucesso.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new Resposta<List<CalculoFreteResponse>>(null, 500, $"Erro: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            return new Resposta<List<CalculoFreteResponse>>(null, 500, $"Erro ao calcular o frete. {ex.Message} ");
        }
        catch (Exception ex)
        {
            return new Resposta<List<CalculoFreteResponse>>(null, 500, $"Erro ao calcular o frete. {ex.Message} ");
        }
    }
}