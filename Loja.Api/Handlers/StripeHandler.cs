using Loja.Core;
using Loja.Core.Handlers;
using Loja.Core.Requisicoes.Stripe;
using Loja.Core.Respostas;
using Loja.Core.Respostas.Stripe;
using Stripe;
using Stripe.Checkout;

namespace Loja.Api.Handlers;

public class StripeHandler : IStripeHandler
{
    public async Task<Resposta<string?>> CriarSessaoAsync(CriarSessaoRequisicao request)
    {
        var options = new SessionCreateOptions
        {
            CustomerEmail = request.UserEmail,
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "Order", request.NumeroDoPedido },
                }
            },
            PaymentMethodTypes = ["card"],
            LineItems = [new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "BRL",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = request.TituloDoProduto,
                        Description = request.DescricaoDoProduto,
                    },
                    UnitAmount = request.ValorDoPedido
                },
                Quantity = 1
            }],
            Mode = "payment",
            SuccessUrl = $"{Configuration.FrontendUrl}/pedidos/{request.NumeroDoPedido}",
            CancelUrl = $""
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        
        return new Resposta<string?>(session.Id);
    }

    public async Task<Resposta<List<StripeRespostaDeTransacao>>> ObterTransacaoPorNumeroPedidoAsync(ObterTransacaoPorNumeroPedidoRequisicao request)
    {
        throw new NotImplementedException();
    }
}