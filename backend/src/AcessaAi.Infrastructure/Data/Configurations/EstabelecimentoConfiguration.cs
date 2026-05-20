using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcessaAi.Infrastructure.Data.Mappings
{
    public class EstabelecimentoConfiguration : IEntityTypeConfiguration<Estabelecimento>
    {
        public void Configure(EntityTypeBuilder<Estabelecimento> builder)
        {
            builder.ToTable("Estabelecimentos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.ComplexProperty(e => e.Geolocalizacao, geo =>
            {
                geo.Property(g => g.Latitude).HasColumnName("Latitude").IsRequired().HasPrecision(9, 6);
                geo.Property(g => g.Longitude).HasColumnName("Longitude").IsRequired().HasPrecision(9, 6);
            });

            builder.ComplexProperty(e => e.Endereco, EnderecoConfiguration.Configure());
            builder.Property(e => e.MediaEstrelas).HasPrecision(3, 2);
            builder.Property(e => e.CadastradoRecente).IsRequired();
            builder.OwnsMany(e => e.Fotos, foto =>
            {
                foto.ToTable("EstabelecimentoFotos");
                foto.HasKey(f => f.Id);
                foto.Property(f => f.Url).IsRequired().HasMaxLength(500);
                foto.Property(f => f.IsCapa).IsRequired().HasDefaultValue(false);
                foto.WithOwner().HasForeignKey(f => f.EstabelecimentoId);
            });

            builder.HasMany(e => e.RecursosAcessibilidade)
                .WithMany()
                .UsingEntity(j => j.ToTable("EstabelecimentoRecursosAcessibilidade"));
        }
    }
}