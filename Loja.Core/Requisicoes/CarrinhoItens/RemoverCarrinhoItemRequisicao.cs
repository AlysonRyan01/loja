using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.CarrinhoItens;

public class RemoverCarrinhoItemRequisicao
{
    [Required (ErrorMessage = "ID é obrigatorio")]
    public long Id { get; set; }
}