using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.CarrinhoItens;

public class AtualizarCarrinhoItemRequisicao
{
    [Required(ErrorMessage = "O ID é obrigatório.")]
    public long Id { get; set; }

    [Required(ErrorMessage = "A quantidade é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}