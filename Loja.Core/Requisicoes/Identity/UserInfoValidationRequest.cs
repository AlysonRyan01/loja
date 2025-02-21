using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Identity;

public class UserInfoValidationRequest
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "O número de telefone informado não é válido.")]
    [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    public string? PhoneNumber { get; set; } = string.Empty;
}