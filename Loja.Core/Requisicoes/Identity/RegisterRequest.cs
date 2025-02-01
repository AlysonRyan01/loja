using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Identity;

public class RegisterRequest
{
    [Required (ErrorMessage = "O Nome deve ser informado")]
    [StringLength(30, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}