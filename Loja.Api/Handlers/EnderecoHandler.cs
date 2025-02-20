using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Respostas;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class EnderecoHandler(LojaDataContext context) : IEnderecoHandler
{
    public async Task<Resposta<Endereco>> ObterEnderecoPorUserId(ObterEnderecoPorUserIdRequisicao request)
    {
        try
        {
            var endereco = await context
                .Enderecos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId.ToString() == request.UserId);

            if (endereco == null)
                return new Resposta<Endereco>(null, 404, "Endereco não encontrado");
            
            return new Resposta<Endereco>(endereco, 200, "Endereco obtido com sucesso");
        }
        catch
        {
            return new Resposta<Endereco>(null, 500, "Erro no servidor");
        }
    }
}