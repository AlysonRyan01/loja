using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos;

public class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produto");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Titulo)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);
        
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500);
        
        builder.Property(x => x.Preco)
            .IsRequired()
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(x => x.Imagens)
            .HasColumnType("VARCHAR");
    }
}