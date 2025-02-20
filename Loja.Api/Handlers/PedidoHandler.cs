using Loja.Api.Data;
using Loja.Core.Enums;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Loja.Core.Requisicoes.Pedidos;
using Loja.Core.Respostas;
using Loja.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class PedidoHandler(
    LojaDataContext context,
    IPedidoItemService pedidoItemService,
    ILogger<ProdutoHandler> logger,
    UserManager<User> userManager) : IPedidoHandler
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
            var user = await userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
            {
                return new Resposta<Pedido?>(null, 404, "Usuario não encontrado.");
            }
            
            if (user.Endereco == null)
            {
                user.Endereco = request.Endereco;
            }
            
            var pedido = new Pedido
            {
                Status = EStatusDoPedido.AguardandoPagamento,
                UserId = request.UserId,
                Itens = new List<PedidoItem>(),
                Endereco = request.Endereco,
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

    public async Task<Resposta<Pedido?>> PagarPedidoAsync(PagarPedidoRequisicao request)
    {
        Pedido? pedido;
        try
        {
            pedido = await context
                .Pedidos
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (pedido == null)
                return new Resposta<Pedido?>(null, 404, "Pedido não encontrado");

            switch (pedido.Status)
            {
                case EStatusDoPedido.Cancelado:
                    return new Resposta<Pedido?>(pedido, 400, "O pedido já foi cancelado e não pode ser pago.");
                
                case EStatusDoPedido.Pago:
                    return new Resposta<Pedido?>(pedido, 400, "O pedido já está pago.");
                
                case EStatusDoPedido.Reembolso:
                    return new Resposta<Pedido?>(pedido, 400, "Esse pedido já foi reembolsado.");
                
                case EStatusDoPedido.AguardandoPagamento:
                    break;
                
                default:
                    return new Resposta<Pedido?>(pedido, 400, "Não foi possível pagar o pedido.");
            }
            
            //codigo do stripe
            
            pedido.Status = EStatusDoPedido.Pago;
            pedido.ExternalReference = request.ExternalReference;
            pedido.AtualizadoEm = DateTime.Now;

            try
            {
                context.Pedidos.Update(pedido);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new Resposta<Pedido?>(pedido, 500, "Falha ao tentar pagar o pedido.");
            }

            return new Resposta<Pedido?>(pedido, 200, $"Pedido {pedido.Numero} pago com sucesso.");
        }
        catch
        {
            return new Resposta<Pedido?>(null, 500, "Falha ao consultar o pedido");
        }
    }

    public async Task<Resposta<Pedido?>> ReembolsarPedidoAsync(ReembolsarPedidoRequisicao request)
    {
        Pedido? pedido;
        try
        {
            pedido = await context
                .Pedidos
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (pedido == null)
                return new Resposta<Pedido?>(null, 404, "Pedido não encontrado");
            
            switch (pedido.Status)
            {
                case EStatusDoPedido.Cancelado:
                    return new Resposta<Pedido?>(pedido, 400, "O pedido já foi cancelado e não pode ser estornado.");
                
                case EStatusDoPedido.Pago:
                    break;
                
                case EStatusDoPedido.Reembolso:
                    return new Resposta<Pedido?>(pedido, 400, "Esse pedido já foi reembolsado.");
                
                case EStatusDoPedido.AguardandoPagamento:
                    return new Resposta<Pedido?>(pedido, 400, "Esse pedido não foi pago e não pode ser reembolsado.");
                
                default:
                    return new Resposta<Pedido?>(pedido, 400, "Não foi possível reembolsar o pedido.");
            }
            
            pedido.Status = EStatusDoPedido.Reembolso;
            pedido.AtualizadoEm = DateTime.Now;

            try
            {
                context.Pedidos.Update(pedido);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new Resposta<Pedido?>(pedido, 400, "Falha ao reembolsar o pagamento.");
            }
            
            return new Resposta<Pedido?>(pedido, 200, $"Pedido {pedido.Numero} estornado com sucesso.");
        }
        catch
        {
            return new Resposta<Pedido?>(null, 500, "Não foi possível recuperar o pedido.");
        }
    }

    public async Task<Resposta<List<Pedido>?>> ObterTodosOsPedidosAsync(ObterTodosOsPedidoRequisicao request)
    {
        try
        {
            var query = context
                .Pedidos
                .AsNoTracking()
                .Include(x => x.Itens)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CriadoEm);
            
            var pedidos = await query.ToListAsync();
            
            return new Resposta<List<Pedido>?>(pedidos, 200, "Pedidos obtidos com sucesso.");
        }
        catch
        {
            return new Resposta<List<Pedido>?>(null, 500, "Não foi possível obter seus pedidos.");
        }
    }

    public async Task<Resposta<Pedido?>> ObterPedidoPeloNumeroAsync(ObterPedidoPeloNumeroRequisicao request)
    {
        try
        {
            var pedido = await context
                .Pedidos
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Numero == request.Numero);
            
            if (pedido == null)
                return new Resposta<Pedido?>(null, 404, "Pedido não encontrado");
            
            return new Resposta<Pedido?>(pedido, 200, "Pedido obtido com sucesso");
        }
        catch
        {
            return new Resposta<Pedido?>(null, 500, "Não foi possível obter o pedido");
        }
    }
}