using Loja.Core.Handlers;
using Loja.Core.Requisicoes.CarrinhoItens;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

[Route("v1/carrinho-item")]
[ApiController]
public class CarrinhoItemController(ICarrinhoItemHandler handler, ILogger<CarrinhoItemController> logger) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create(CriarCarrinhoItemRequisicao requisicao)
    {
        try
        {
            var user = HttpContext.User;

            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));

            var result = await handler.CriarCarrinhoItemAsync(requisicao, user);

            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }

    [HttpPut("update/{id:long}")]
    public async Task<IActionResult> Update(long id, AtualizarCarrinhoItemRequisicao requisicao)
    {
        try
        {
            var user = HttpContext.User;

            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));

            requisicao.Id = id;
            var result = await handler.AtualizarrCarrinhoItemAsync(requisicao, user);

            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var user = HttpContext.User;

            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));

            var requisicao = new RemoverCarrinhoItemRequisicao { Id = id };
            var result = await handler.RemoverCarrinhoItemAsync(requisicao, user);

            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var user = HttpContext.User;

            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>("Erro de validação", 400, "Erro de validação"));

            var result = await handler.ObterCarrinhoItemAsync(user);

            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest(new Resposta<string>("Erro no servidor", 400, e.Message));
        }
    }
}
