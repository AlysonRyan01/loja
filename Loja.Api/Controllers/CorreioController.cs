using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Correios;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[ApiController]
public class CorreioController(ICorreioHandler handler) : ControllerBase
{
    [HttpPost("v1/correios/frete")]
    public async Task<IActionResult> ObterFrete(CalcularFreteRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));

            var result = await handler.CalcularFreteAsync(request);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        catch (Exception e)
        {
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }
}