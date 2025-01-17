using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers;

public class ProdutoController : ControllerBase
{
    private readonly IProdutoHandler _produtoHandler;
    
    public ProdutoController(IProdutoHandler produtoHandler)
    {
        _produtoHandler = produtoHandler;
    }
    
    [HttpPost("v1/produto/")]
    public async Task<IActionResult> Post([FromForm] CriarProdutoRequisicao request,
        [FromServices] LojaDataContext context)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Erro no ModelState");

            var result = await _produtoHandler.CriarProdutoAsync(request);
            
            return result.IsSuccess
                ? Created($"v1/produtos/{result.Dados.Id}", new Resposta<dynamic>(new
                {
                    result.Dados.Id,
                    result.Dados.Titulo,
                    result.Dados.Descricao,
                    result.Dados.Preco,
                    ImagensTotais = result.Dados.Imagens.Count,
                }, 201, "Produto criado com sucesso!"))
                : StatusCode(500, "Algo deu errado");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}