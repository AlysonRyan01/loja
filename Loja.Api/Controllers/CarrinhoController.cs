using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[Route("v1/carrinho/")]
[ApiController]
public class CarrinhoController(ICarrinhoHandler handler, ILogger<CarrinhoController> logger) : ControllerBase
{
    [HttpGet("/")]
    public async Task<IActionResult> GetById()
    {
        try
        {
            var user = HttpContext.User;
            
            if(!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validacao", 401, "Erro de validacao"));
            
            var result = await handler.ObterCarrinhoPorUserAsync(user);
            
            return result.IsSuccess ? Ok(result) : BadRequest(result);
                
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no obter", 401, e.Message));
        }
    }
}