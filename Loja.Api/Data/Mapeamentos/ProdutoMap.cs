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
        
        builder.Property(x => x.Marca)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50);
        
        builder.Property(x => x.Modelo)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50);
        
        builder.Property(x => x.Serie)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(30);
        
        builder.Property(x => x.Tamanho)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(10);
        
        builder.Property(x => x.Garantia)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(10);
        
        builder.Property(x => x.Preco)
            .IsRequired(true)
            .HasColumnType("MONEY");
        
        builder.Property(x => x.IsActive)
            .IsRequired(true)
            .HasColumnType("BIT");
        
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Largura)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(x => x.Altura)
            .IsRequired()
            .HasColumnType("INT");
    }
}