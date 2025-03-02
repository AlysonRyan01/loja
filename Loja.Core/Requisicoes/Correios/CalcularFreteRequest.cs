using System.ComponentModel.DataAnnotations;

namespace Loja.Core.Requisicoes.Correios;

public class CalcularFreteRequest
{
    [Required(ErrorMessage = "O CEP de origem é obrigatório.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP de origem deve ter 8 dígitos.")]
    public string CepOrigem { get; set; } = "83601000";

    [Required(ErrorMessage = "O CEP de destino é obrigatório.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP de destino deve ter 8 dígitos.")]
    public string CepDestino { get; set; } = string.Empty;

    [Required(ErrorMessage = "O peso é obrigatório.")]
    [Range(0.01, 30, ErrorMessage = "O peso deve estar entre 0,01kg e 30kg.")]
    public string Peso { get; set; } = "6";

    [Required(ErrorMessage = "O comprimento é obrigatório.")]
    [Range(16, 105, ErrorMessage = "O comprimento deve estar entre 16cm e 105cm.")]
    public string Comprimento { get; set; } = string.Empty;

    [Required(ErrorMessage = "A altura é obrigatória.")]
    [Range(2, 105, ErrorMessage = "A altura deve estar entre 2cm e 105cm.")]
    public string Altura { get; set; } = string.Empty;

    [Required(ErrorMessage = "A largura é obrigatória.")]
    [Range(11, 105, ErrorMessage = "A largura deve estar entre 11cm e 105cm.")]
    public string Largura { get; set; } = "25";
}