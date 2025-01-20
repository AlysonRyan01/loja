using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Controllers;

[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoHandler _produtoHandler;
    private readonly ILogger<ProdutoController> _logger;
    
    public ProdutoController(IProdutoHandler produtoHandler, ILogger<ProdutoController> logger)
    {
        _produtoHandler = produtoHandler;
        _logger = logger;
    }

    [HttpPost("v1/produto/criar")]
    public async Task<IActionResult> Post([FromForm] CriarProdutoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));

            var result = await _produtoHandler.CriarProdutoAsync(request);

            return result.IsSuccess
                ? Created($"v1/produtos/{result.Dados.Id}", new Resposta<dynamic>(new
                {
                    result.Dados.Id,
                    result.Dados.Titulo,
                    result.Dados.Descricao,
                    result.Dados.Preco,
                    ImagensTotais = result.Dados.Imagens.Count,
                }, 201, result.Mensagem))
                : StatusCode(500, new Resposta<dynamic>(null, 500, result.Mensagem));
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpPut("v1/produto/")]
    public async Task<IActionResult> Put([FromForm] AtualizarProdutoRequisicao request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));

            var result = await _produtoHandler.AtualizarProdutoAsync(request);

            return result.IsSuccess
                ? Ok(new Resposta<dynamic>(new
                {
                    result.Dados.Id,
                    result.Dados.Titulo,
                    result.Dados.Descricao,
                    result.Dados.Preco,
                    ImagensTotais = result.Dados.Imagens.Count,
                }, 200, result.Mensagem))
                : StatusCode(500, new Resposta<dynamic>(null, 500, result.Mensagem));
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpDelete("v1/produto/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));

            var result = await _produtoHandler.RemoverProdutoAsync(id);

            return result.IsSuccess
                ? Ok(new Resposta<dynamic>(new
                {
                    result.Dados.Id,
                    result.Dados.Titulo,
                    result.Dados.Descricao,
                    result.Dados.Preco,
                    ImagensTotais = result.Dados.Imagens.Count,
                }, 200, result.Mensagem))
                : StatusCode(500, new Resposta<dynamic>(null, 500, result.Mensagem));
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/produto/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var result = await _produtoHandler.ObterProdutoPorIdAsync(id);

            return result.IsSuccess
                ? Ok(new Resposta<Produto>(result.Dados
                , 200, result.Mensagem))
                : StatusCode(500, new Resposta<dynamic>(null, 500, result.Mensagem));
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
    
    [HttpGet("v1/produto/")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new Resposta<dynamic>(null, 400, "Erro de validação nos dados fornecidos"));
            
            var result = await _produtoHandler.ObterTodosProdutos();

            return result.IsSuccess
                ? Ok(new Resposta<List<Produto>>(result.Dados
                    , 200, result.Mensagem))
                : StatusCode(500, new Resposta<dynamic>(null, 500, result.Mensagem));
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro ao salvar no banco de dados: {Mensagem}", dbEx.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro ao salvar os dados no banco de dados"));
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Erro de argumento inválido: {Mensagem}", argEx.Message);
            return BadRequest(new Resposta<dynamic>(null, 400, "Argumentos inválidos fornecidos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Mensagem}", ex.Message);
            return StatusCode(500, new Resposta<dynamic>(null, 500, "Erro interno do servidor"));
        }
    }
}