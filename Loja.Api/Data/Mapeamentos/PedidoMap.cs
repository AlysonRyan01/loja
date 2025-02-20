using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class PedidoMap : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedido");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Numero)
            .IsRequired(true)
            .HasColumnType("CHAR")
            .HasMaxLength(8);
        
        builder.Property(x => x.ExternalReference)
            .IsRequired(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(60);
        
        builder.Property(x => x.Gateway)
            .IsRequired(true)
            .HasColumnType("SMALLINT");
        
        builder.Property(x => x.CriadoEm)
            .IsRequired(true)
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.AtualizadoEm)
            .IsRequired(true)
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.Status)
            .IsRequired(true)
            .HasColumnType("SMALLINT");
        
        builder.Property(x => x.UserId)
            .IsRequired(true)
            .HasColumnType("INT")
            .HasMaxLength(160);

        builder.Ignore(x => x.ValorTotal);
        
        builder.HasMany(x => x.Itens)
            .WithOne(x => x.Pedido)
            .HasForeignKey(x => x.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Endereco)
            .WithOne(e => e.Pedido) 
            .HasForeignKey<Endereco>(e => e.PedidoId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}