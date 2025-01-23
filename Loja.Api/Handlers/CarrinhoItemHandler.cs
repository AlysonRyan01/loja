using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class CarrinhoItemHandler(
    LojaDataContext context,
    ILogger<ProdutoHandler> logger) : ICarrinhoItemHandler
{
    public async Task<Resposta<CarrinhoItem>> CriarCarrinhoItemAsync(CriarCarrinhoItemRequisicao requisicao)
    {
        try
        {
            var produto = await context.Produtos.Include(x => x.Imagens).FirstOrDefaultAsync(x => x.Id == requisicao.ProdutoId);
            
            var carrinho = await context.Carrinhos
                .Include(x => x.CarrinhoItens)
                .FirstOrDefaultAsync(x => x.Id == requisicao.CarrinhoId);

            if (carrinho == null || produto == null)
                return new Resposta<CarrinhoItem>(null, 401, "Produto ou carrinho invalidos.");
            
            var carrinhoItem = new CarrinhoItem
            {
                ProdutoId = produto.Id,
                CarrinhoId = carrinho.Id,
                Quantidade = requisicao.Quantidade,
                PrecoTotal = produto.Preco * requisicao.Quantidade,
            };
            
            carrinho.ValorTotal += carrinhoItem.PrecoTotal;
            
            await context.AddAsync(carrinhoItem);
            await context.SaveChangesAsync();
            
            return new Resposta<CarrinhoItem>(carrinhoItem, 201, "CarrinhoItem criado com sucesso.");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Erro ao salvar o carrinho item no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Ocorreu um erro inesperado ao cadastrar o carrinho item");
        }
    }

    public Task<Resposta<CarrinhoItem>> AtualizarrCarrinhoItemAsync(AtualizarCarrinhoItemRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<CarrinhoItem>> RemoverCarrinhoItemAsync(RemoverCarrinhoItemRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<CarrinhoItem>> ObterCarrinhoItemPorIdAsync(ObterCarrinhoItemPorIdRequisicao requisicao)
    {
        throw new NotImplementedException();
    }

    public Task<Resposta<List<CarrinhoItem>?>> ObterTodosCarrinhoItensAsync(ObterTodosCarrinhoItensRequisicao requisicao)
    {
        throw new NotImplementedException();
    }
}