using Loja.Core.Enums;

namespace Loja.Core.Models;

public class Pedido
{
    public long Id { get; set; }
    
    public string Numero { get; set; } = Guid.NewGuid().ToString("N")[..8];

    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public DateTime AtualizadoEm { get; set; } = DateTime.Now;

    public EPaymentGateway Gateway { get; set; } = EPaymentGateway.Stripe;
    public string? ExternalReference { get; set; }

    public EStatusDoPedido Status { get; set; } = EStatusDoPedido.AguardandoPagamento;

    public List<PedidoItem> Itens { get; set; }
    
    public string UserId { get; set; }
    
    public Endereco Endereco { get; set; } = new();
    
    public decimal ValorTotal => Itens?.Sum(item => item.PrecoTotal) ?? 0;
}