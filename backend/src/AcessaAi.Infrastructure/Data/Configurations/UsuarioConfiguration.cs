using AcessaAi.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcessaAi.Infrastructure.Data.Mappings
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.NomeCompleto)
                .IsRequired()
                .HasMaxLength(300);
            builder.Property(x => x.DataNascimento)
                .IsRequired();
            builder.Property(x => x.Ativo).IsRequired().HasDefaultValue(true);
            builder.ComplexProperty(x => x.Endereco, EnderecoConfiguration.Configure());
        }
    }
}
