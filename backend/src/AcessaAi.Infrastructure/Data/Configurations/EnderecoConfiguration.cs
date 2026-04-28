using AcessaAi.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcessaAi.Infrastructure.Data.Mappings
{
    public static class EnderecoConfiguration
    {
        public static Action<ComplexPropertyBuilder<Endereco>> Configure()
        {
            return builder =>
            {
                builder.Property(e => e.Logradouro).HasColumnName("Logradouro").HasMaxLength(200);
                builder.Property(e => e.UF).HasColumnName("UF").HasMaxLength(50);
                builder.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(20);
                builder.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(100);
                builder.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(100);
                builder.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(100);
                builder.Property(e => e.CEP).HasColumnName("CEP").IsRequired().HasMaxLength(20);
            };
        }

    }
}
