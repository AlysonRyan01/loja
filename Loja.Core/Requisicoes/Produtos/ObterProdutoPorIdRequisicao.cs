using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Produtos;

public class ObterProdutoPorSlugRequisicao
{
    [Required(ErrorMessage = "Id do produto deve ser informado")]
    public string Slug { get; set; }
}