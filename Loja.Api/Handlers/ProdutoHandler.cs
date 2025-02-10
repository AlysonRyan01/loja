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
                Marca = requisicao.Marca,
                Modelo = requisicao.Modelo,
                Serie = requisicao.Serie,
                Tamanho = requisicao.Tamanho,
                Garantia = requisicao.Garantia,
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
        using var transaction = await context.Database.BeginTransactionAsync();
    
        try
        {
            var produto = await context.Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == requisicao.Id);
            
            var carrinhoItens = await context.CarrinhoItens
                .AsNoTracking()
                .Where(x => x.ProdutoId == requisicao.Id)
                .ToListAsync();

            if (produto == null)
                return new Resposta<Produto?>(null, 404, "Produto não encontrado");
            
            if (requisicao.Imagens.Any())
            {
                var novasImagens = await uploadImagemService.UploadImagem(requisicao.Imagens);
                await context.Imagens.AddRangeAsync(novasImagens);
                produto.Imagens.AddRange(novasImagens);
            }
            
            produto.Titulo = requisicao.Titulo;
            produto.Marca = requisicao.Marca;
            produto.Modelo = requisicao.Modelo;
            produto.Serie = requisicao.Serie;
            produto.Tamanho = requisicao.Tamanho;
            produto.Garantia = requisicao.Garantia;
            produto.Descricao = requisicao.Descricao;
            produto.Preco = requisicao.Preco;

            context.Produtos.Update(produto);

            if (carrinhoItens.Any())
            {
                foreach (var carrinhoItem in carrinhoItens)
                {
                    carrinhoItem.PrecoTotal = produto.Preco * carrinhoItem.Quantidade;
                }
                context.CarrinhoItens.UpdateRange(carrinhoItens);
            }
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Resposta<Produto?>(produto, 200, "Produto atualizado com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            await transaction.RollbackAsync();
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> RemoverProdutoAsync(RemoverProdutoRequisicao requisicao)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var produto = await context
                .Produtos
                .Include(x => x.Imagens)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == requisicao.Id);
            
            if (produto == null)
                return new Resposta<Produto?>(null, 404, "Produto não encontrado");

            var carrinhoItens = await context.CarrinhoItens
                .AsNoTracking()
                .Where(x => x.ProdutoId == requisicao.Id)
                .ToListAsync();
            
            if (carrinhoItens.Any())
                context.CarrinhoItens.RemoveRange(carrinhoItens);

            if (produto.Imagens.Any())
            {
                var imagens = produto.Imagens.ToList();
                context.Imagens.RemoveRange(imagens);
                uploadImagemService.ExcluirImagem(imagens);
            }
            
            context.Produtos.Remove(produto);
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new Resposta<Produto?>(produto, 200, "Produto removido com sucesso");
        }
        catch (DbUpdateException dbEx)
        {
            await transaction.RollbackAsync();
            logger.LogError(dbEx.Message);
            return new Resposta<Produto?>(null, 500, "Erro ao salvar o produto no banco de dados");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex.Message);
            return new Resposta<Produto?>(null, 500, "Ocorreu um erro inesperado ao cadastrar o produto");
        }
    }

    public async Task<Resposta<Produto?>> ObterProdutoPorIdAsync(ObterProdutoPorIdRequisicao requisicao)
    {
        try
        {
            var produto = await context
                .Produtos
                .Include(x => x.Imagens)
                .FirstOrDefaultAsync(p => p.Id == requisicao.Id);
            
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