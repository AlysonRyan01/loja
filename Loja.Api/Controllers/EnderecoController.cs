using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Endereco;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[ApiController]
public class EnderecoController(IEnderecoHandler handler) : ControllerBase
{
    [HttpGet("v1/endereco/detalhes/{userId}")]
    public async Task<IActionResult> GetById(string userId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 401, "Erro de validação"));
            
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new Resposta<string>("Erro de validação", 401, "Erro de validação"));

            var request = new ObterEnderecoPorUserIdRequisicao
            {
                UserId = userId
            };

            var result = await handler.ObterEnderecoPorUserId(request);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            return BadRequest(new Resposta<string>("Erro no obter", 401, e.Message));
        }
    }
}