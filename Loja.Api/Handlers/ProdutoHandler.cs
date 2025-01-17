using Loja.Api.Data;
using Loja.Core.Handlers;
using Loja.Core.Models;
using Loja.Core.Requisicoes.Produtos;
using Loja.Core.Respostas;
using Loja.Core.Services;

namespace Loja.Api.Handlers;

public class ProdutoHandler : IProdutoHandler
{
    private readonly IUploadImagemService _uploadImagemService;
    private readonly LojaDataContext _context;
    
    public ProdutoHandler(IUploadImagemService uploadImagemService, LojaDataContext context)
    {
        _uploadImagemService = uploadImagemService;
        _context = context;
    }
    
    public async Task<Resposta<Produto?>> CriarProdutoAsync(CriarProdutoRequisicao requisicao)
    {
        try
        {
            var imagens = await _uploadImagemService.UploadImagem(requisicao.Imagens);
            
            if (imagens == null)
                return new Resposta<Produto?>(null, 500, "Pelo menos uma imagem deve ser adicionada ao produto");

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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}