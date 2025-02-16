using Loja.Core.Models;
using Loja.Core.Services;

namespace Loja.Api.Services;

public class PedidoItemService : IPedidoItemService
{
    public List<PedidoItem>? CriarPedidoItemViaCarrinho(Carrinho carrinho, Pedido pedido)
    {
        try
        {
            var pedidoItens = new List<PedidoItem>();
            
            foreach (var CarrinhoItem in carrinho.CarrinhoItens)
            {
                var pedidoItem = new PedidoItem
                {
                    ProdutoId = CarrinhoItem.Produto.Id,
                    NomeProduto = CarrinhoItem.Produto.Titulo,
                    PrecoUnitario = CarrinhoItem.Produto.Preco,
                    Quantidade = CarrinhoItem.Quantidade,
                    Pedido = pedido,
                    PedidoId = pedido.Id
                };
                pedidoItens.Add(pedidoItem);
            }
            
            if(!pedidoItens.Any())
                return null;
            
            return pedidoItens;
        }
        catch
        {
            return null;
        }
    }
    
    public PedidoItem? CriarPedidoItemViaProduto(Produto produto, Pedido pedido)
    {
        try
        {
            var pedidoItem = new PedidoItem
            {
                ProdutoId = produto.Id,
                NomeProduto = produto.Titulo,
                PrecoUnitario = produto.Preco,
                Quantidade = 1,
                Pedido = pedido,
                PedidoId = pedido.Id
            };
            
            return pedidoItem;
        }
        catch
        {
            return null;
        }
    }
}