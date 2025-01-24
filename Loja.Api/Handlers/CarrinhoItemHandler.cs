using System.Security.Claims;
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
    public async Task<Resposta<CarrinhoItem>> CriarCarrinhoItemAsync(CriarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        try
        {
            var produto = await context.Produtos.Include(x => x.Imagens).FirstOrDefaultAsync(x => x.Id == requisicao.ProdutoId);
            
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
                return new Resposta<CarrinhoItem>(null, 401, "Usuário não autenticado");
            
            if (!long.TryParse(userIdClaim, out long userId))
                return new Resposta<CarrinhoItem>(null, 400, "ID de usuário inválido");
            
            var carrinho = await context.Carrinhos
                .Include(x => x.CarrinhoItens)
                .FirstOrDefaultAsync(x => x.UserId == userId);

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

    public async Task<Resposta<CarrinhoItem>> AtualizarrCarrinhoItemAsync(AtualizarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
                return new Resposta<CarrinhoItem>(null, 401, "Usuário não autenticado");
            
            if (!long.TryParse(userIdClaim, out long userId))
                return new Resposta<CarrinhoItem>(null, 400, "ID de usuário inválido");
            
            var carrinhoItem = await context.CarrinhoItens
                .Include(x => x.Produto)
                .Include(x => x.Carrinho)
                .FirstOrDefaultAsync(x => x.Id == requisicao.Id && x.Carrinho.UserId == userId);

            if (carrinhoItem?.Carrinho == null)
                return new Resposta<CarrinhoItem>(null, 404, "Produto ou carrinho inválido.");

            var carrinho = carrinhoItem.Carrinho;
            
            var diferencaQuantidade = requisicao.Quantidade - carrinhoItem.Quantidade;
            carrinhoItem.Quantidade = requisicao.Quantidade;
            carrinhoItem.PrecoTotal += carrinhoItem.Produto.Preco * diferencaQuantidade;

            carrinho.ValorTotal += carrinhoItem.Produto.Preco * diferencaQuantidade;

            context.CarrinhoItens.Update(carrinhoItem);
            context.Carrinhos.Update(carrinho);
            await context.SaveChangesAsync();

            return new Resposta<CarrinhoItem>(carrinhoItem, 200, "CarrinhoItem atualizado com sucesso.");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Erro ao salvar o carrinho item no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Ocorreu um erro inesperado ao atualizar o carrinho item");
        }
    }

    public async Task<Resposta<CarrinhoItem>> RemoverCarrinhoItemAsync(RemoverCarrinhoItemRequisicao requisicao, ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
                return new Resposta<CarrinhoItem>(null, 401, "Usuário não autenticado");
            
            if (!long.TryParse(userIdClaim, out long userId))
                return new Resposta<CarrinhoItem>(null, 400, "ID de usuário inválido");
            
            var carrinhoItem = await context.CarrinhoItens
                .Include(x => x.Produto)
                .Include(x => x.Carrinho)
                .FirstOrDefaultAsync(x => x.Id == requisicao.Id && x.Carrinho.UserId == userId);

            if (carrinhoItem == null)
                return new Resposta<CarrinhoItem>(null, 404, "CarrinhoItem não encontrado.");

            if (carrinhoItem?.Carrinho == null)
                return new Resposta<CarrinhoItem>(null, 404, "Carrinho inválido.");

            var carrinho = carrinhoItem.Carrinho;
            
            carrinho.ValorTotal -= carrinhoItem.PrecoTotal;

            context.CarrinhoItens.Remove(carrinhoItem);
            context.Carrinhos.Update(carrinho);
            await context.SaveChangesAsync();
            
            return new Resposta<CarrinhoItem>(carrinhoItem, 200, "CarrinhoItem removido com sucesso.");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Erro ao remover o carrinho item no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<CarrinhoItem>(null, 500, "Ocorreu um erro inesperado ao remover o carrinho item");
        }
    }

    public async Task<Resposta<List<CarrinhoItem>?>> ObterCarrinhoItemAsync(ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
                return new Resposta<List<CarrinhoItem>?>(null, 401, "Usuário não autenticado");
            
            if (!long.TryParse(userIdClaim, out long userId))
                return new Resposta<List<CarrinhoItem>?>(null, 400, "ID de usuario invalido");
            
            var carrinhoItem = await context.CarrinhoItens
                .Include(x => x.Produto)
                    .ThenInclude(p => p.Imagens)
                .Include(x => x.Carrinho)
                .Where(x => x.Carrinho.UserId == userId)
                .ToListAsync();
            
            if(!carrinhoItem.Any())
                return new Resposta<List<CarrinhoItem>?>(null, 404, "Nenhum carrinhoItem encontrado");
            
            return new Resposta<List<CarrinhoItem>?>(carrinhoItem, 200, "CarrinhoItem(s) obtido(s) com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<List<CarrinhoItem>?>(null, 500, "Erro ao retornar os carrinhosItens do banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<List<CarrinhoItem>?>(null, 500, "Ocorreu um erro inesperado ao retornar os carrinhos itens");
        }
    }
}