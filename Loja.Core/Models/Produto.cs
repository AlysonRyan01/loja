namespace Loja.Core.Models;

public class Produto
{
    public long Id { get; set; }
    public string Titulo { get; set; } = String.Empty;
    public string Descricao { get; set; } = String.Empty;
    public decimal Preco { get; set; }
    public List<string> Imagens { get; set; } = new();
}