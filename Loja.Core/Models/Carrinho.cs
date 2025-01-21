namespace Loja.Core.Models;

public class Carrinho
{
    public long Id { get; set; }
    public List<CarrinhoItem> CarrinhoItens { get; set; } = new();
}