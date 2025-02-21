using Loja.Core.Models.Identity;

namespace Loja.Core.Models;

public class Endereco
{
    public long Id { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? CEP { get; set; }
    public string? Pais { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }

    public long? PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
}