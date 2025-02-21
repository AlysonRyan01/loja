using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Endereco;

public class AtualizarEnderecoRequisicao
{
    [Required(ErrorMessage = "A Rua é obrigatória.")]
    [StringLength(100, ErrorMessage = "A Rua deve ter no máximo 100 caracteres.")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "O Número é obrigatório.")]
    [StringLength(10, ErrorMessage = "O Número deve ter no máximo 10 caracteres.")]
    public string Numero { get; set; }
    
    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "A Cidade é obrigatória.")]
    [StringLength(50, ErrorMessage = "A Cidade deve ter no máximo 50 caracteres.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O Estado é obrigatório.")]
    [StringLength(50, ErrorMessage = "O Estado deve ter no máximo 50 caracteres.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000.")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "O País é obrigatório.")]
    [StringLength(50, ErrorMessage = "O País deve ter no máximo 50 caracteres.")]
    public string Pais { get; set; }

    [Required(ErrorMessage = "O UserId é obrigatório.")]
    public string UserId { get; set; }

    public long? PedidoId { get; set; }
}