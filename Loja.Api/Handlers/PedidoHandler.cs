using System.Data.Common;
using Loja.Api.Data;
using Loja.Core.Enums;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Pedidos;
using Loja.Core.Respostas;
using Loja.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class PedidoHandler(
    LojaDataContext context,
    IPedidoItemService pedidoItemService,
    ILogger<ProdutoHandler> logger) : IPedidoHandler
{
    public async Task<Resposta<Pedido?>> CancelarPedidoAsync(PedidoCanceladoRequisicao request)
    {
        try
        {
            var pedido = await context
                .Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (pedido == null)
                return new Resposta<Pedido?>(null, 404, "Não foi possível cancelar o pedido");

            switch (pedido.Status)
            {
                case EStatusDoPedido.Cancelado:
                    return new Resposta<Pedido?>(pedido, 400, "O pedido já foi cancelado!");

                case EStatusDoPedido.Pago:
                    return new Resposta<Pedido?>(pedido, 400, "Um pedido pago não pode ser cancelado!");

                case EStatusDoPedido.Reembolso:
                    return new Resposta<Pedido?>(pedido, 400, "Um pedido reembolsado não pode ser cancelado");

                case EStatusDoPedido.AguardandoPagamento:
                    break;

                default:
                    return new Resposta<Pedido?>(pedido, 400, "Pedido com situação inválida!");
            }

            pedido.Status = EStatusDoPedido.Cancelado;
            pedido.AtualizadoEm = DateTime.Now;

            try
            {
                context.Pedidos.Update(pedido);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new Resposta<Pedido?>(pedido, 500, "Não foi possível atualizar seu pedido");
            }

            return new Resposta<Pedido?>(pedido, 200, $"Pedido {pedido.Numero} atualizado!");
        }
        catch
        {
            return new Resposta<Pedido?>(null, 500, "Não foi possível cancelar o pedido");
        }
    }

    public async Task<Resposta<Pedido?>> CriarPedidoAsync(CriarPedidoRequisicao request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var pedido = new Pedido
            {
                Status = EStatusDoPedido.AguardandoPagamento,
                UserId = request.UserId,
                Itens = new List<PedidoItem>()
            };

            await context.Pedidos.AddAsync(pedido);
            await context.SaveChangesAsync();

            if (request.CarrinhoId != 0)
            {
                var carrinho = await context
                    .Carrinhos
                    .AsNoTracking()
                    .Include(x => x.CarrinhoItens)
                    .ThenInclude(x => x.Produto)
                    .FirstOrDefaultAsync(x => x.Id == request.CarrinhoId);

                if (carrinho == null)
                    return new Resposta<Pedido?>(null, 404, "Carrinho não encontrado.");

                var pedidoItens = pedidoItemService.CriarPedidoItemViaCarrinho(carrinho, pedido);

                if (pedidoItens == null || !pedidoItens.Any())
                    return new Resposta<Pedido?>(null, 400, "Falha ao criar os itens do pedido.");

                pedido.Itens.AddRange(pedidoItens);
            }
            else if (request.ProdutoId != 0)
            {
                var produto = await context
                    .Produtos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ProdutoId && x.IsActive == true);

                if (produto == null)
                    return new Resposta<Pedido?>(null, 404, "Produto não encontrado.");

                var pedidoItem = pedidoItemService.CriarPedidoItemViaProduto(produto, pedido);

                if (pedidoItem == null)
                    return new Resposta<Pedido?>(null, 400, "Falha ao criar o item do pedido.");

                pedido.Itens.Add(pedidoItem);
            }

            await context.SaveChangesAsync(); 
            await transaction.CommitAsync(); 

            return new Resposta<Pedido?>(pedido, 201, "Pedido criado com sucesso!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new Resposta<Pedido?>(null, 500, $"Erro ao criar o pedido: {ex.Message}");
        }
    }

    public Task<Resposta<Pedido?>> PagarPedidoAsync(PagarPedidoRequisicao request)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<Pedido?>> ReembolsarPedidoAsync(ReembolsarPedidoRequisicao request)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<List<Pedido>?>> ObterTodosOsPedidosAsync(ObterTodosOsPedidoRequisicao request)
    {
        throw new NotImplementedException();
    }
}