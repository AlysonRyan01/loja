using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Loja.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class ProdutoHandler : IProdutoHandler
{
    private readonly IUploadImagemService _uploadImagemService;
    private readonly LojaDataContext _context;
    private readonly ILogger<ProdutoHandler> _logger;
    
    public ProdutoHandler(IUploadImagemService uploadImagemService, LojaDataContext context, ILogger<ProdutoHandler> logger)
    {
        _uploadImagemService = uploadImagemService;
        _context = context;
        _logger = logger;
    }

    public async Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao)
    {
        try
        {
            var imagens = await _uploadImagemService.UploadImagem(requisicao.Imagens);

            var produto = new Produto
            {
                Titulo = requisicao.Titulo,
                Descricao = requisicao.Descricao,
                Preco = requisicao.Preco,
                Imagens = imagens
            };

            foreach (var imagem in produto.Imagens)
            {
                imagem.ProdutoId = produto.Id;
            }

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return new Resposta<Produto?>(produto, 201, "Produto cadastrado com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> AtualizarProdutoAsync(AtualizarProdutoRequisicao requisicao)
    {
        try
        {
            var produto = await _context.Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == requisicao.Id);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            if (requisicao.Imagens.Any())
            {
                var novasImagens = await _uploadImagemService.UploadImagem(requisicao.Imagens);
                produto.Imagens.AddRange(novasImagens);
            }
            
            var imagens = await _uploadImagemService.UploadImagem(requisicao.Imagens);
            
            produto.Titulo = requisicao.Titulo;
            produto.Descricao = requisicao.Descricao;
            produto.Preco = requisicao.Preco;
            produto.Imagens = imagens;
            
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            
            return new Resposta<Produto?>(produto, 200, "Produto atualizado com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> RemoverProdutoAsync(int id)
    {
        try
        {
            var produto = await _context
                .Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            
            return new Resposta<Produto?>(produto, 200, "Produto removido com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> ObterProdutoPorIdAsync(int id)
    {
        try
        {
            var produto = await _context
                .Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            return new Resposta<Produto?>(produto);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<List<Produto?>>> ObterTodosProdutos()
    {
        try
        {
            var produtos = await _context.Produtos.Include(x => x.Imagens).AsNoTracking().ToListAsync();

            if (!produtos.Any())
                return new Resposta<List<Produto?>>(null, 404, "Nenhum produto encontrado");
            
            return new Resposta<List<Produto?>>(produtos);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx.Message);
            return new Resposta<List<Produto?>>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Resposta<List<Produto?>>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }
}