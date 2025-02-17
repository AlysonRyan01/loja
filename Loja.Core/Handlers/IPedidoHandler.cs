using Loja.Core.Models;
using Loja.Core.Requisicoes.Pedidos;
using Loja.Core.Respostas;

namespace Loja.Core.Handlers;

public interface IPedidoHandler
{
    Task<Resposta<Pedido?>> CancelarPedidoAsync(PedidoCanceladoRequisicao request);
    Task<Resposta<Pedido?>> CriarPedidoAsync(CriarPedidoRequisicao request);
    Task<Resposta<Pedido?>> PagarPedidoAsync(PagarPedidoRequisicao request);
    Task<Resposta<Pedido?>> ReembolsarPedidoAsync(ReembolsarPedidoRequisicao request);
    Task<Resposta<List<Pedido>?>> ObterTodosOsPedidosAsync(ObterTodosOsPedidoRequisicao request);
    Task<Resposta<Pedido?>> ObterPedidoPeloNumeroAsync(ObterPedidoPeloNumeroRequisicao request);
}