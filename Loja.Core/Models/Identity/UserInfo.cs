namespace Loja.Core.Models.Identity;

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public string? PhoneNumber { get; set; } = string.Empty;
    public Endereco? Endereco { get; set; }
    public Dictionary<string, string> Claims { get; set; } = [];
}