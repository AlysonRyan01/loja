using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Loja.Core.Requisicoes.Produtos;

public class AtualizarProdutoRequisicao
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "A marca é obrigatória.")]
    [StringLength(50, ErrorMessage = "A marca deve ter no máximo 50 caracteres.")]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [StringLength(50, ErrorMessage = "O modelo deve ter no máximo 50 caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número de série é obrigatório.")]
    [StringLength(30, ErrorMessage = "O número de série deve ter no máximo 30 caracteres.")]
    public string Serie { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tamanho da TV é obrigatório.")]
    [StringLength(30, ErrorMessage = "O tamanho deve ter no máximo 30 caracteres.")]
    public string Tamanho { get; set; } = string.Empty;

    [Required(ErrorMessage = "A garantia é obrigatória.")]
    [StringLength(30, ErrorMessage = "A garantia deve ter no máximo 30 caracteres.")]
    public string Garantia { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [MinLength(1, ErrorMessage = "O produto deve ter pelo menos uma imagem.")]
    public List<IFormFile> Imagens { get; set; } = new();
    
    [Required(ErrorMessage = "Informe se o produto está ativo")]
    public bool IsActive { get; set; } = true;
    
    [Required(ErrorMessage = "A largura é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A largura deve ser maior que zero.")]
    public int Largura { get; set; }

    [Required(ErrorMessage = "A altura é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A altura deve ser maior que zero.")]
    public int Altura { get; set; }
}