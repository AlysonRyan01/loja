using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Carrinhos;

public class CriarCarrinhoRequisicao
{
    [Required(ErrorMessage = "O UserId é obrigatório.")]
    public long UserId { get; set; }
}