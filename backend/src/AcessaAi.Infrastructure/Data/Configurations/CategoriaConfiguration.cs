using AcessaAi.Domain.Categorias.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcessaAi.Infrastructure.Data.Mappings
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.UrlIcone)
                .HasMaxLength(255);
        }
    }
}
