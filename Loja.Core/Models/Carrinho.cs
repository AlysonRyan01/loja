using Loja.Core.Models.Identity;

namespace Loja.Core.Models;

public class Carrinho
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    
    public List<CarrinhoItem> CarrinhoItens { get; set; }
    
    public decimal ValorTotal => CarrinhoItens.Sum(item => item.PrecoTotal);
}