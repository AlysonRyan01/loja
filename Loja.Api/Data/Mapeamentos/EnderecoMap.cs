using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class EnderecoMap : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("Enderecos");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Rua)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(e => e.Numero)
            .IsRequired(false)
            .HasMaxLength(10);

        builder.Property(e => e.Cidade)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(e => e.Estado)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.CEP)
            .IsRequired(false)
            .HasMaxLength(20);

        builder.Property(e => e.Pais)
            .IsRequired(false)
            .HasMaxLength(50);
        
        builder.HasOne(e => e.User)
            .WithOne(u => u.Endereco)
            .HasForeignKey<Endereco>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(e => e.Pedido) // Relacionamento 1-1 entre Endereco e Pedido
            .WithOne(p => p.Endereco) // Relacionamento 1-1
            .HasForeignKey<Endereco>(e => e.PedidoId) // Chave estrangeira no Endereco
            .IsRequired(false) // Tornar a chave estrangeira opcional
            .OnDelete(DeleteBehavior.SetNull); // Quando o Pedido 
    }
}