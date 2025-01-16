using System.Text.Json.Serialization;

namespace Loja.Core.Respostas;

public class Resposta <T>
{
    [JsonConstructor]
    public Resposta()
    {
        _code = Configuration.DefaultStatusCode;
    }
    
    public Resposta(T? dados, int code = Configuration.DefaultStatusCode, string? mensagem = null)
    {
        Dados = dados;
        Mensagem = mensagem;
        _code = code;
    }

    private readonly int _code;
    
    public T? Dados { get; set; }
    public string? Mensagem { get; set; }
    
    [JsonIgnore]
    public bool IsSuccess 
        => _code >= 200 && _code <= 299;
}