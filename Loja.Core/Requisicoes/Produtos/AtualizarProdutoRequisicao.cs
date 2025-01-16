using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Produtos;

public class AtualizarProdutoRequisicao
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [MinLength(1, ErrorMessage = "O produto deve ter pelo menos uma imagem.")]
    public List<string> Imagens { get; set; } = new();
}