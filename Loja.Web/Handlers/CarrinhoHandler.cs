using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Respostas;

namespace Dima.Web.Handlers;

public class CarrinhoHandler : ICarrinhoHandler
{
    public Task<Resposta<Carrinho>> ObterCarrinhoPorUserAsync(ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }
}