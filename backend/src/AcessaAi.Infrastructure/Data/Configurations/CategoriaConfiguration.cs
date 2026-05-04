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
            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Icone)
                .HasMaxLength(255);

            var dataCadastro = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(
                new Categoria
                {
                    Id = 1,
                    Nome = "Rampa de Acesso",
                    Descricao = "Entrada principal nivelada ou com rampa.",
                    Icone = "fa-wheelchair-move",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 2,
                    Nome = "Banheiro Adaptado",
                    Descricao = "Banheiro com barras de apoio e espaço para manobra.",
                    Icone = "fa-restroom",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 3,
                    Nome = "Elevador",
                    Descricao = "Elevador ou plataforma elevatória para acesso aos andares.",
                    Icone = "fa-elevator",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 4,
                    Nome = "Estacionamento Reservado",
                    Descricao = "Vagas demarcadas e próximas à entrada.",
                    Icone = "fa-square-parking",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 5,
                    Nome = "Piso Tátil",
                    Descricao = "Piso de alerta e direcional para pessoas com deficiência visual.",
                    Icone = "fa-person-walking-with-cane",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 6,
                    Nome = "Braille",
                    Descricao = "Cardápios ou sinalizações em Braille.",
                    Icone = "fa-braille",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 7,
                    Nome = "Libras",
                    Descricao = "Equipe com conhecimento básico ou intérprete de Libras.",
                    Icone = "fa-hands",
                    DataCadastro = dataCadastro
                },
                new Categoria
                {
                    Id = 8,
                    Nome = "Ambiente Calmo",
                    Descricao = "Local com baixo estímulo sonoro e visual (Amigável para TEA).",
                    Icone = "fa-volume-xmark",
                    DataCadastro = dataCadastro
                }
            );
        }
    }
}
