using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Loja.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Handlers;

public class ProdutoHandler(
    IUploadImagemService uploadImagemService,
    LojaDataContext context,
    ILogger<ProdutoHandler> logger)
    : IProdutoHandler
{
    public async Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao)
    {
        try
        {
            var imagens = await uploadImagemService.UploadImagem(requisicao.Imagens);

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

            await context.Produtos.AddAsync(produto);
            await context.SaveChangesAsync();

            return new Resposta<Produto?>(produto, 201, "Produto cadastrado com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> AtualizarProdutoAsync(AtualizarProdutoRequisicao requisicao)
    {
        try
        {
            var produto = await context.Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == requisicao.Id);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            if (requisicao.Imagens.Any())
            {
                var novasImagens = await uploadImagemService.UploadImagem(requisicao.Imagens);
                produto.Imagens.AddRange(novasImagens);
            }
            
            var imagens = await uploadImagemService.UploadImagem(requisicao.Imagens);
            
            produto.Titulo = requisicao.Titulo;
            produto.Descricao = requisicao.Descricao;
            produto.Preco = requisicao.Preco;
            produto.Imagens = imagens;
            
            context.Produtos.Update(produto);
            await context.SaveChangesAsync();
            
            return new Resposta<Produto?>(produto, 200, "Produto atualizado com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> RemoverProdutoAsync(int id)
    {
        try
        {
            var produto = await context
                .Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            
            var imagens = await context.Imagens.Where(x => x.ProdutoId == id).ToListAsync();

            uploadImagemService.ExcluirImagem(imagens);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();
            
            return new Resposta<Produto?>(produto, 200, "Produto removido com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> ObterProdutoPorIdAsync(int id)
    {
        try
        {
            var produto = await context
                .Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if(produto == null)
                return new Resposta<Produto?>(null, 404, "Produto nao encontrado");
            
            return new Resposta<Produto?>(produto);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<List<Produto>?>> ObterTodosProdutos()
    {
        try
        {
            var produtos = await context.Produtos.Include(x => x.Imagens).AsNoTracking().ToListAsync();

            if (!produtos.Any())
                return new Resposta<List<Produto>?>(null, 404, "Nenhum produto encontrado");
            
            return new Resposta<List<Produto>?>(produtos);
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx.Message);
            return new Resposta<List<Produto>?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Resposta<List<Produto>?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }
}