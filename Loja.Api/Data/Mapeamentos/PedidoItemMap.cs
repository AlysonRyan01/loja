using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class PedidoItemMap : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.ToTable("PedidoItem");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeProduto)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);

        builder.Property(x => x.PrecoUnitario)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(x => x.Quantidade)
            .IsRequired();

        builder.Ignore(x => x.PrecoTotal);
        
        builder.HasOne(x => x.Pedido)
            .WithMany(x => x.Itens)
            .HasForeignKey(x => x.PedidoId);
    }
}