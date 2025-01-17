using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class CarrinhoItemMap : IEntityTypeConfiguration<CarrinhoItem>
{
    public void Configure(EntityTypeBuilder<CarrinhoItem> builder)
    {
        builder.ToTable("CarrinhoItem");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CarrinhoId)
            .IsRequired()
            .HasColumnType("BIGINT");

        builder.Property(x => x.Quantidade)
            .IsRequired()
            .HasColumnType("INT");

        builder.HasOne(x => x.Produto)
            .WithMany()
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}