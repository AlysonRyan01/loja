using Loja.Core.Handlers;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[Route("v1/CarrinhoItem/")]
[ApiController]
public class CarrinhoItemController(ICarrinhoItemHandler handler, ILogger<CarrinhoItemController> logger) : ControllerBase
{
    [HttpPost("/criar")]
    public async Task<IActionResult> Post(CriarCarrinhoItemRequisicao requisicao)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 401, "Erro de validacao"));
            
            var result = await handler.CriarCarrinhoItemAsync(requisicao);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no criar", 401, e.Message));
        }
    }
}