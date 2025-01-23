using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.CarrinhoItens;

public class CriarCarrinhoItemRequisicao
{
    [Required(ErrorMessage = "O ID do produto é obrigatório.")]
    public long ProdutoId { get; set; }
    
    public long CarrinhoId { get; set; }

    [Required(ErrorMessage = "A quantidade é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}