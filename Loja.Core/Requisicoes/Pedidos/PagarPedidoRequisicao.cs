namespace Loja.Core.Requisicoes.Pedidos;

public class PagarPedidoRequisicao : RequisicaoBase
{
    public long Id { get; set; }
    public string ExternalReference { get; set; }
    public string NumeroDoPedido { get; set; }
}