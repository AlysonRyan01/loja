using Loja.Core.Models;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface IEnderecoHandler
{
    Task<Resposta<Endereco>> ObterEnderecoPorUserId(ObterEnderecoPorUserIdRequisicao request);
}