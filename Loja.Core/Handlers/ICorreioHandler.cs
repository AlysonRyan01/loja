using Loja.Core.Requisicoes.Correios;
using Loja.Core.Respostas;
using Loja.Core.Respostas.MelhorEnvio;

namespace Loja.Core.Handlers;

public interface ICorreioHandler
{
    Task<Resposta<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request);
}