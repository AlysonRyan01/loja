using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Produtos;

public class ObterProdutoPorIdRequisicao
{
    [Required(ErrorMessage = "Id do produto deve ser informado")]
    public long Id { get; set; }
}