using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Endereco;

public class ObterEnderecoPorUserIdRequisicao
{
    [Required]
    public string UserId { get; set; }
}