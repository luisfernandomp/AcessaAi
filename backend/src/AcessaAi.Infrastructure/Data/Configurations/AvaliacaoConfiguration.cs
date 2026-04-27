using AcessaAi.Domain.Avaliacoes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcessaAi.Infrastructure.Data.Mappings
{
    public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder.ToTable("Avaliacoes");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Estrelas).IsRequired();
            builder.Property(x => x.Comentario).IsRequired().HasMaxLength(300);
            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UsuarioId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
