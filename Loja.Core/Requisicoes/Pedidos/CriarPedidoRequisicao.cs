using Loja.Core.Models;

namespace Loja.Core.Requisicoes.Pedidos;

public class CriarPedidoRequisicao : RequisicaoBase
{
    public List<PedidoItem> PedidoItens { get; set; }
}