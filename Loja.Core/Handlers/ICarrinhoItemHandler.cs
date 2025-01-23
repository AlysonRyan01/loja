using Loja.Core.Models;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface ICarrinhoItemHandler
{
    Task<Resposta<CarrinhoItem>> CriarCarrinhoItemAsync(CriarCarrinhoItemRequisicao requisicao);
    Task<Resposta<CarrinhoItem>> AtualizarrCarrinhoItemAsync(AtualizarCarrinhoItemRequisicao requisicao);
    Task<Resposta<CarrinhoItem>> RemoverCarrinhoItemAsync(RemoverCarrinhoItemRequisicao requisicao);
    Task<Resposta<CarrinhoItem>> ObterCarrinhoItemPorIdAsync(ObterCarrinhoItemPorIdRequisicao requisicao);
    Task<Resposta<List<CarrinhoItem>?>> ObterTodosCarrinhoItensAsync(ObterTodosCarrinhoItensRequisicao requisicao);
}