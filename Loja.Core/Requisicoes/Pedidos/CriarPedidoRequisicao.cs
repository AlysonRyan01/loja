using Loja.Core.Models;

namespace Loja.Core.Requisicoes.Pedidos;

public class CriarPedidoRequisicao : RequisicaoBase
{
    public long CarrinhoId { get; set; }
    public long ProdutoId { get; set; }
    public Models.Endereco Endereco { get; set; }
}