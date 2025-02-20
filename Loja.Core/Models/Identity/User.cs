using Microsoft.AspNetCore.Identity;

namespace Loja.Core.Models.Identity;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; } = new();
    public Endereco? Endereco { get; set; }
}