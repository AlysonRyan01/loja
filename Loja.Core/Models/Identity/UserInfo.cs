namespace Loja.Core.Models.Identity;

public class UserInfo
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
}