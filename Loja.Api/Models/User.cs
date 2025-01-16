using Microsoft.AspNetCore.Identity;

namespace Loja.Api.Models;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; }
}