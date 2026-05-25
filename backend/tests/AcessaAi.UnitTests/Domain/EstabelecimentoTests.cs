using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using AcessaAi.Domain.Usuarios.Entities;
using FluentAssertions;
using Xunit;

namespace AcessaAi.UnitTests.Domain;

public class EstabelecimentoTests
{
    private static Geocordenadas GeoValida() => new(-23.5505, -46.6333);
    private static Endereco EnderecoValido() => new("Rua A", "SP", "São Paulo", "100", "01310-100", "Centro");

    [Fact]
    public void Criar_ComDadosValidos_RetornaEstabelecimento()
    {
        var resultado = Estabelecimento.Criar("Farmácia Central", TipoEstabelecimento.Farmacia, GeoValida(), EnderecoValido());

        resultado.IsError.Should().BeFalse();
        resultado.Value.Nome.Should().Be("Farmácia Central");
        resultado.Value.Tipo.Should().Be(TipoEstabelecimento.Farmacia);
        resultado.Value.CadastradoRecente.Should().BeTrue();
        resultado.Value.MediaEstrelas.Should().Be(0);
        resultado.Value.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Criar_ComNomeVazio_RetornaErro()
    {
        var resultado = Estabelecimento.Criar("", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido());

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Estabelecimento.NomeObrigatorio");
    }

    [Fact]
    public void Criar_ComGeolocalizacaoNula_RetornaErro()
    {
        var resultado = Estabelecimento.Criar("Nome Válido", TipoEstabelecimento.Restaurante, null!, EnderecoValido());

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Estabelecimento.GeocordenadasObrigatorio");
    }

    [Fact]
    public void Criar_ComNomeVazioEGeoNula_RetornaAmbosOsErros()
    {
        var resultado = Estabelecimento.Criar("", TipoEstabelecimento.Restaurante, null!, EnderecoValido());

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Estabelecimento.NomeObrigatorio");
        resultado.Errors.Should().Contain(e => e.Code == "Estabelecimento.GeocordenadasObrigatorio");
    }

    [Fact]
    public void Alterar_ComDadosValidos_AtualizaPropriedades()
    {
        var estabelecimento = Estabelecimento.Criar("Nome Antigo", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;
        var novaGeo = new Geocordenadas(-22.9068, -43.1729);

        var resultado = estabelecimento.Alterar("Nome Novo", novaGeo, TipoEstabelecimento.Shopping);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Nome.Should().Be("Nome Novo");
        resultado.Value.Geolocalizacao.Should().Be(novaGeo);
        resultado.Value.Tipo.Should().Be(TipoEstabelecimento.Shopping);
        resultado.Value.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void Alterar_SemTipo_MantemTipoOriginal()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Banco, GeoValida(), EnderecoValido()).Value;

        estabelecimento.Alterar("Nome Atualizado", GeoValida());

        estabelecimento.Tipo.Should().Be(TipoEstabelecimento.Banco);
    }

    [Fact]
    public void Desativar_DesativaEstabelecimentoERegistraDataAtualizacao()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Banco, GeoValida(), EnderecoValido()).Value;

        estabelecimento.Desativar();

        estabelecimento.Ativo.Should().BeFalse();
        estabelecimento.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void AdicionarAvaliacao_ComAvaliacaoValida_AtualizaMediaEstrelas()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;
        var usuario = new Usuario("Maria", "maria@email.com", DateTimeOffset.UtcNow.AddYears(-30));
        var avaliacao = Avaliacao.Criar("Bom!", 4, usuario, estabelecimento).Value;

        var resultado = estabelecimento.AdicionarAvaliacao(avaliacao);

        resultado.IsError.Should().BeFalse();
        resultado.Value.MediaEstrelas.Should().Be(4);
        resultado.Value.Avaliacoes.Should().HaveCount(1);
    }

    [Fact]
    public void AdicionarAvaliacao_ComMultiplasAvaliacoes_CalculaMediaCorreta()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;
        var usuario = new Usuario("Maria", "maria@email.com", DateTimeOffset.UtcNow.AddYears(-30));
        var av1 = Avaliacao.Criar("Ótimo", 5, usuario, estabelecimento).Value;
        var av2 = Avaliacao.Criar("Regular", 3, usuario, estabelecimento).Value;

        estabelecimento.AdicionarAvaliacao(av1);
        estabelecimento.AdicionarAvaliacao(av2);

        estabelecimento.MediaEstrelas.Should().Be(4);
        estabelecimento.Avaliacoes.Should().HaveCount(2);
    }

    [Fact]
    public void AdicionarImagem_PrimeiraImagem_EhCapa()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;

        estabelecimento.AdicionarImagem("https://cdn.example.com/foto1.jpg");

        estabelecimento.Fotos.Should().HaveCount(1);
        estabelecimento.Fotos.First().IsCapa.Should().BeTrue();
    }

    [Fact]
    public void AdicionarImagem_SegundaImagem_NaoEhCapa()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;
        estabelecimento.AdicionarImagem("https://cdn.example.com/foto1.jpg");

        estabelecimento.AdicionarImagem("https://cdn.example.com/foto2.jpg");

        estabelecimento.Fotos.Should().HaveCount(2);
        estabelecimento.Fotos.Last().IsCapa.Should().BeFalse();
    }

    [Fact]
    public void AdicionarImagem_ComFlagCapa_DefinidoComoCapa()
    {
        var estabelecimento = Estabelecimento.Criar("Nome", TipoEstabelecimento.Restaurante, GeoValida(), EnderecoValido()).Value;
        estabelecimento.AdicionarImagem("https://cdn.example.com/foto1.jpg");

        estabelecimento.AdicionarImagem("https://cdn.example.com/foto2.jpg", isCapa: true);

        estabelecimento.Fotos.Last().IsCapa.Should().BeTrue();
    }
}
