using Loja.Core.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Loja.Api.Models;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; }

    public long CarrinhoId { get; set; }
    public Carrinho Carrinho { get; set; }
}