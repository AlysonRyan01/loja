namespace Loja.Core.Respostas.MelhorEnvio;

public class CalculoFreteResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Currency { get; set; }
    public int DeliveryTime { get; set; } 
    public DeliveryRange DeliveryRange { get; set; } 
    public ShippingCompany Company { get; set; } 
}