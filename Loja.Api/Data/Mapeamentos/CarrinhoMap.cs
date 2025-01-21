using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class CarrinhoMap : IEntityTypeConfiguration<Carrinho>
{
    public void Configure(EntityTypeBuilder<Carrinho> builder)
    {
        builder.ToTable("Carrinho");

        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.CarrinhoItens)
            .WithOne(x => x.Carrinho)
            .HasForeignKey(x => x.CarrinhoId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}