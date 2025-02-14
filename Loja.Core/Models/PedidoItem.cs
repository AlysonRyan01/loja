namespace Loja.Core.Models;

public class PedidoItem
{
    public long Id { get; set; }
    
    public long PedidoId { get; set; }
    public Pedido Pedido { get; set; }
    
    public long ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoTotal => PrecoUnitario * Quantidade;
}