using Loja.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Loja.Api.Models;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; } = new();

    public long CarrinhoId { get; set; }
    public Carrinho Carrinho { get; set; }
}