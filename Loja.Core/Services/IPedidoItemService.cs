using Loja.Core.Models;

namespace Loja.Core.Services;

public interface IPedidoItemService
{
    List<PedidoItem>? CriarPedidoItemViaCarrinho(Carrinho carrinho, Pedido pedido);
    PedidoItem? CriarPedidoItemViaProduto(Produto produto, Pedido pedido);
}