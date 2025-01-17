using Loja.Core.Models;
using Loja.Core.Respostas;
using Loja.Core.Requisicoes.Produtos;

namespace Loja.Core.Handlers;

public interface IProdutoHandler
{
    Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao);
}