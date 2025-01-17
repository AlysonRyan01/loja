using Loja.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Api.Data.Mapeamentos
{
    public class ImagemMap : IEntityTypeConfiguration<Imagem>
    {
        public void Configure(EntityTypeBuilder<Imagem> builder)
        {
            builder.ToTable("Imagem");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Url)
                .IsRequired()
                .HasColumnType("TEXT");
            
            builder.HasOne(x => x.Produto)
                .WithMany(p => p.Imagens) 
                .HasForeignKey(x => x.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ProdutoId)
                .IsRequired()
                .HasColumnType("BIGINT");
        }
    }
}