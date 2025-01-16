namespace Loja.Core.Models;

public class Carrinho
{
    public long Id { get; set; }
    public List<CarrinhoItem> CarrinhoItems { get; set; } = new();
    public long UserId { get; set; }
}