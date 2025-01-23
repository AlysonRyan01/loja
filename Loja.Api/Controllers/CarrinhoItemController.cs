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
            var user = HttpContext.User;
            
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 400, "Erro de validacao"));
            
            var result = await handler.CriarCarrinhoItemAsync(requisicao, user);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }
    
    [HttpPut("/editar")]
    public async Task<IActionResult> Put(AtualizarCarrinhoItemRequisicao requisicao)
    {
        try
        {
            var user = HttpContext.User;
            
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 400, "Erro de validacao"));
            
            var result = await handler.AtualizarrCarrinhoItemAsync(requisicao, user);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }
    
    
}
