using System.Security.Claims;
using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Respostas;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class CarrinhoHandler(
    LojaDataContext context,
    ILogger<ProdutoHandler> logger) : ICarrinhoHandler
{
    public async Task<Resposta<Carrinho>> ObterCarrinhoPorUserAsync(ClaimsPrincipal user)
    {
        try
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim))
                return new Resposta<Carrinho>(null, 401, "Usuário não autenticado");
            
            if (!long.TryParse(userIdClaim, out long userId))
                return new Resposta<Carrinho>(null, 400, "ID de usuário inválido");
            
            var carrinho = await context.Carrinhos
                .Include(x => x.CarrinhoItens)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            
            if(carrinho == null)
                return new Resposta<Carrinho>(null, 401, "Carrinho nao encontrado");

            decimal valorTotalCarrinho = 0;

            foreach (var item in carrinho.CarrinhoItens)
                valorTotalCarrinho += item.PrecoTotal;
            
            carrinho.ValorTotal = valorTotalCarrinho;
            
            return new Resposta<Carrinho>(carrinho, 200, "Carrinho obtido com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<Carrinho>(null, 500, "Erro ao retornar o carrinho pelo banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<Carrinho>(null, 500, "Ocorreu um erro inesperado ao retornar o carrinho");
        }
    }
}