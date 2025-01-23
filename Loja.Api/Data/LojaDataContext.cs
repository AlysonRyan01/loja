using System.Reflection;
using Loja.Core.Models;
using Loja.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Data;

public class LojaDataContext(DbContextOptions<LojaDataContext> options) : IdentityDbContext
    <
        User,
        IdentityRole<long>, long, IdentityUserClaim<long>,
        IdentityUserRole<long>, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>
    >(options)
{
    public DbSet<Carrinho> Carrinhos { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<CarrinhoItem> CarrinhoItens { get; set; } = null!;
    public DbSet<Imagem> Imagens { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}