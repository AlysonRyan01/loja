namespace Loja.Core.Models;

public class CarrinhoItem
{
    public long Id { get; set; }
    
    public Produto Produto { get; set; } = new();
    public long ProdutoId { get; set; }
    
    public int Quantidade { get; set; }
    
    public Carrinho Carrinho { get; set; } = new();
    public long CarrinhoId { get; set; }
    
    public decimal PrecoTotal => Produto.Preco * Quantidade;

}