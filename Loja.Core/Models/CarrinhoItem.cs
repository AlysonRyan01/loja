namespace Loja.Core.Models;

public class CarrinhoItem
{
    public long Id { get; set; }
    public Produto Produto { get; set; } = new();
    public int Quantidade { get; set; }
    
    public decimal PrecoTotal => Produto.Preco * Quantidade;

    public Carrinho Carrinho { get; set; }
    public long CarrinhoId { get; set; }
    public long ProdutoId { get; set; }
}