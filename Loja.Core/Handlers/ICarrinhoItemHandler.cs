using System.Security.Claims;
using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface ICarrinhoItemHandler
{
    Task<Resposta<CarrinhoItem>> CriarCarrinhoItemAsync(CriarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user);
    Task<Resposta<CarrinhoItem>> AtualizarrCarrinhoItemAsync(AtualizarCarrinhoItemRequisicao requisicao, ClaimsPrincipal user);
    Task<Resposta<CarrinhoItem>> RemoverCarrinhoItemAsync(RemoverCarrinhoItemRequisicao requisicao, ClaimsPrincipal user);
    Task<Resposta<List<CarrinhoItem>?>> ObterCarrinhoItemAsync(ClaimsPrincipal user);
}