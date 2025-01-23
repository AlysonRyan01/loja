namespace Loja.Core.Models;

public class Produto
{
    public long Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public List<Imagem> Imagens { get; set; }
}