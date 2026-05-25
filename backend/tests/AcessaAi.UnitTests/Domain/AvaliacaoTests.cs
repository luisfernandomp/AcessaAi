using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Enums;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using AcessaAi.Domain.Usuarios.Entities;
using FluentAssertions;
using Xunit;

namespace AcessaAi.UnitTests.Domain;

public class AvaliacaoTests
{
    private static Usuario CriarUsuarioValido() =>
        new("João Silva", "joao@email.com", DateTimeOffset.UtcNow.AddYears(-25));

    private static Estabelecimento CriarEstabelecimentoValido()
    {
        var geo = new Geocordenadas(-23.5505, -46.6333);
        var endereco = new Endereco("Rua A", "SP", "São Paulo", "100", "01310-100", "Centro");
        return Estabelecimento.Criar("Restaurante Teste", TipoEstabelecimento.Restaurante, geo, endereco).Value;
    }

    [Fact]
    public void Criar_ComDadosValidos_RetornaAvaliacao()
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();

        var resultado = Avaliacao.Criar("Ótimo lugar!", 5, usuario, estabelecimento);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Comentario.Should().Be("Ótimo lugar!");
        resultado.Value.QuantidadeEstrelas.Should().Be(5);
        resultado.Value.Ativo.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Criar_ComEstrelasForaDoIntervalo_RetornaErro(ushort estrelas)
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();

        var resultado = Avaliacao.Criar("Comentário válido", estrelas, usuario, estabelecimento);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.NotaForaDoIntervalo");
    }

    [Fact]
    public void Criar_ComComentarioVazio_RetornaErro()
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();

        var resultado = Avaliacao.Criar("", 3, usuario, estabelecimento);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.ComentarioObrigatorio");
    }

    [Fact]
    public void Criar_ComComentarioMuitoLongo_RetornaErro()
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();
        var comentarioLongo = new string('x', 501);

        var resultado = Avaliacao.Criar(comentarioLongo, 3, usuario, estabelecimento);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.ComentarioMuitoLongo");
    }

    [Fact]
    public void Criar_ComUsuarioNulo_RetornaErro()
    {
        var estabelecimento = CriarEstabelecimentoValido();

        var resultado = Avaliacao.Criar("Comentário", 3, null!, estabelecimento);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.UsuarioIdObrigatorio");
    }

    [Fact]
    public void Criar_ComEstabelecimentoNulo_RetornaErro()
    {
        var usuario = CriarUsuarioValido();

        var resultado = Avaliacao.Criar("Comentário", 3, usuario, null!);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.EstabelecimentoIdObrigatorio");
    }

    [Fact]
    public void Criar_ComMultiplosErros_RetornaTodosOsErros()
    {
        var resultado = Avaliacao.Criar("", 0, null!, null!);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public void Alterar_ComDadosValidos_AtualizaPropriedades()
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();
        var avaliacao = Avaliacao.Criar("Comentário antigo", 3, usuario, estabelecimento).Value;

        var resultado = avaliacao.Alterar("Novo comentário", 4);

        resultado.IsError.Should().BeFalse();
        resultado.Value.Comentario.Should().Be("Novo comentário");
        resultado.Value.QuantidadeEstrelas.Should().Be(4);
        resultado.Value.DataAtualizacao.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Alterar_ComEstrelasForaDoIntervalo_RetornaErro(ushort estrelas)
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();
        var avaliacao = Avaliacao.Criar("Comentário válido", 3, usuario, estabelecimento).Value;

        var resultado = avaliacao.Alterar("Comentário válido", estrelas);

        resultado.IsError.Should().BeTrue();
        resultado.Errors.Should().Contain(e => e.Code == "Avaliacao.NotaForaDoIntervalo");
    }

    [Fact]
    public void Excluir_DesativaAvaliacaoERegistraDataAtualizacao()
    {
        var usuario = CriarUsuarioValido();
        var estabelecimento = CriarEstabelecimentoValido();
        var avaliacao = Avaliacao.Criar("Comentário", 3, usuario, estabelecimento).Value;

        avaliacao.Excluir();

        avaliacao.Ativo.Should().BeFalse();
        avaliacao.DataAtualizacao.Should().NotBeNull();
    }
}
