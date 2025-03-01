using System.Xml.Linq;

namespace Dima.Web.Handlers;

public class CorreiosHandler(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    
    public async Task<string> CalcularFreteAsync(string cepOrigem, string cepDestino, double peso)
    {
        string url = $"https://ws.correios.com.br/calculador/CalcPrecoPrazo.aspx?" +
                     $"sCepOrigem={cepOrigem}&sCepDestino={cepDestino}&nVlPeso={peso}&" +
                     $"nCdFormato=1&nVlComprimento=20&nVlAltura=5&nVlLargura=15&" +
                     $"nCdServico=40010,41106&StrRetorno=xml";

        var response = await _httpClient.GetStringAsync(url);
        
        var xml = XDocument.Parse(response);
        var valorFrete = xml.Descendants("Valor").FirstOrDefault()?.Value;

        return valorFrete ?? "Erro ao calcular frete";
    }
}