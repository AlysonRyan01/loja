namespace Loja.Core.Models;

public class Produto
{
    public long Id { get; set; }
    public string Titulo { get; set; }
    public string Slug { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Serie { get; set; }
    public string Tamanho { get; set; }
    public string Garantia { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public List<Imagem> Imagens { get; set; }
    public int Largura { get; set; }
    public int Altura { get; set; }
    public bool IsActive { get; set; }
}