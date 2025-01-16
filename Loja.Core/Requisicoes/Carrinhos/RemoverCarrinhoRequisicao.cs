using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Carrinhos;

public class RemoverCarrinhoRequisicao
{
    [Required(ErrorMessage = "O Id é obrigatório.")]
    public long Id { get; set; }
}