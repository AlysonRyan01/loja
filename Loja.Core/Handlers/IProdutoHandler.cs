using Loja.Core.Models;
using Loja.Core.Respostas;
using Loja.Core.Requisicoes.Produtos;

namespace Loja.Core.Handlers;

public interface IProdutoHandler
{
    Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao);
    Task<Resposta<Produto?>> AtualizarProdutoAsync(AtualizarProdutoRequisicao requisicao);
    Task<Resposta<Produto?>> RemoverProdutoAsync(RemoverProdutoRequisicao requisicao);
    Task<Resposta<Produto?>> ObterProdutoPorSlugAsync(ObterProdutoPorSlugRequisicao requisicao);
    Task<Resposta<List<Produto>?>> ObterTodosProdutos();
}