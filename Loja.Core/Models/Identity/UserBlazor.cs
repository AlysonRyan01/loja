namespace Loja.Core.Models.Identity;

public class UserBlazor
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public Dictionary<string, string> Claims { get; set; } = [];
}