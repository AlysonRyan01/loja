using System.Text.Json.Serialization;

namespace Loja.Core.Respostas;

public class RespostaPaginada <T> : Resposta <T>
{
    [JsonConstructor]
    public RespostaPaginada(T? dados, int contadorTotal, int tamanhoDaPagina = Configuration.DefaultPageSize, int paginaAtual = 1) : base(dados)
    {
        Dados = dados;
        ContadorTotal = contadorTotal;
        TamanhoDaPagina = tamanhoDaPagina;
        PaginaAtual = paginaAtual;
    }

    public RespostaPaginada(T? dados, int code = Configuration.DefaultStatusCode, string? mensagem = null) : base(dados, code, mensagem)
    {
        
    }
    
    public int PaginaAtual { get; set; }
    public int PaginasTotais => (int)Math.Ceiling(ContadorTotal / (double)TamanhoDaPagina);
    public int TamanhoDaPagina { get; set; } = Configuration.DefaultPageSize;
    public int ContadorTotal { get; set; }
}