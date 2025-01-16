using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Carrinhos;

public class AtualizarCarrinhoRequisicao
{
    [Required(ErrorMessage = "O Id é obrigatório.")]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "O UserId é obrigatório.")]
    public long UserId { get; set; }
    
    [Required(ErrorMessage = "O CarrinhoItemId é obrigatório.")]
    public long CarrinhoItemId { get; set; }

    public int NovaQuantidade { get; set; }
}