using System.Text.Json.Serialization;

namespace Loja.Core.Models;

public class CarrinhoItem
{
    public long Id { get; set; }
    
    public Produto Produto { get; set; }
    public long ProdutoId { get; set; }
    
    public int Quantidade { get; set; }
    
    [JsonIgnore]
    public Carrinho Carrinho { get; set; }
    public long CarrinhoId { get; set; }
    
    public decimal PrecoTotal { get; set; }

}