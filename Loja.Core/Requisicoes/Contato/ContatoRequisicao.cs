using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Contato;

public class ContatoRequisicao
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [EmailAddress(ErrorMessage = "Telefone inválido.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    public string Mensagem { get; set; }
}