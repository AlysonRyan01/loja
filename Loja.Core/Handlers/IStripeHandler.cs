using Loja.Core.Requisicoes.Stripe;
using Loja.Core.Respostas;
using Loja.Core.Respostas.Stripe;

namespace Loja.Core.Handlers;

public interface IStripeHandler
{
    Task<Resposta<string?>> CriarSessaoAsync(CriarSessaoRequisicao request);
    Task<Resposta<List<StripeRespostaDeTransacao>>> ObterTransacaoPorNumeroPedidoAsync(ObterTransacaoPorNumeroPedidoRequisicao request);
}