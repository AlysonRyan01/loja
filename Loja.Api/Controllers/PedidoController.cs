using System.Security.Claims;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Pedidos;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Controllers;

[ApiController]
public class PedidoController(IPedidoHandler handler) : ControllerBase
{
    [HttpPost("v1/pedido/create")]
    public async Task<IActionResult> CreateAsync(CriarPedidoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;

            request.UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            
            var result = await handler.CriarPedidoAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }

    [HttpPost("v1/pedido/pagar/{id:long}")]
    public async Task<IActionResult> PagarAsync(long id, PagarPedidoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;

            request.UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            request.Id = id;
            
            var result = await handler.PagarPedidoAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
     
    [HttpPost("v1/pedido/cancelar/{id:long}")]
    public async Task<IActionResult> CancelarPedidoAsync(long id, PedidoCanceladoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;
            
            request.Id = id;
            request.UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            
            var result = await handler.CancelarPedidoAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpPost("v1/pedido/reembolsar/{id:long}")]
    public async Task<IActionResult> ReembolsarPedidoAsync(long id, ReembolsarPedidoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;
            
            request.UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            request.Id = id;
            
            var result = await handler.ReembolsarPedidoAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/pedido/obter")]
    public async Task<IActionResult> ObterTodosOsPedidosAsync()
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;

            var request = new ObterTodosOsPedidoRequisicao
            {
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };
            
            var result = await handler.ObterTodosOsPedidosAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/pedido/{numero}/")]
    public async Task<IActionResult> ObterPedidoPeloNumero(string numero)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var user = HttpContext.User;

            var request = new ObterPedidoPeloNumeroRequisicao
            {
                Numero = numero,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };
            
            var result = await handler.ObterPedidoPeloNumeroAsync(request);
            
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException)
        {
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception)
        {
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
}