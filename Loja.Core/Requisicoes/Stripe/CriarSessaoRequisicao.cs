namespace Loja.Core.Requisicoes.Stripe;

public class CriarSessaoRequisicao
{
    public string UserEmail { get; set; } = string.Empty;
    public string NumeroDoPedido { get; set; } = String.Empty;
    public string TituloDoProduto { get; set; } = String.Empty;
    public string DescricaoDoProduto { get; set; } = String.Empty;
    public long ValorDoPedido { get; set; }
}