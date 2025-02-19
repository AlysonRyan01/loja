using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Controllers;

[ApiController]
public class ProdutoController(IProdutoHandler produtoHandler, ILogger<ProdutoController> logger)
    : ControllerBase
{
    [HttpPost("v1/produto/create")]
    public async Task<IActionResult> Post([FromForm] CriarProdutoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));

            var result = await produtoHandler.CriarProdutoAsync(request);

            return result.IsSuccess
                ? Created($"v1/produto/{result.Dados?.Id}", result)
                : StatusCode(500, result);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpPut("v1/produto/update/{id:long}")]
    public async Task<IActionResult> Put(long id, [FromForm] AtualizarProdutoRequisicao requisicao)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<string>(null, 400, "Erro de validação nos dados fornecidos"));
            
            requisicao.Id = id;

            var result = await produtoHandler.AtualizarProdutoAsync(requisicao);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode(500, result);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpDelete("v1/produto/delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var requisicao = new RemoverProdutoRequisicao
            {
                Id = id
            };

            var result = await produtoHandler.RemoverProdutoAsync(requisicao);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode(500, result);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/produto/{slug}")]
    public async Task<IActionResult> GetById(string slug)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var requisicao = new ObterProdutoPorSlugRequisicao
            {
                Slug = slug
            };
            
            var result = await produtoHandler.ObterProdutoPorSlugAsync(requisicao);

            return result.IsSuccess
                ? Ok(result)
                : StatusCode(500, result);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/produto/list")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var result = await produtoHandler.ObterTodosProdutos();

            return result.IsSuccess
                ? Ok(result)
                : StatusCode(500, result);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<string>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<string>(null, 500, "Erro interno do servidor"));
        }
    }
}