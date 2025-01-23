using System.Security.Claims;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Requisicoes.Carrinhos;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface ICarrinhoHandler
{
    Task<Resposta<Carrinho>> ObterCarrinhoPorUserAsync(ClaimsPrincipal user);
}