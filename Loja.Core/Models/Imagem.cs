namespace Loja.Core.Models;

public class Imagem
{
    public long Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public long ProdutoId { get; set; } 
    public Produto Produto { get; set; } = null!;
}