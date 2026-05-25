using AcessaAi.Domain.Usuarios.Entities;
using FluentAssertions;
using Xunit;

namespace AcessaAi.UnitTests.Domain;

public class UsuarioTests
{
    [Fact]
    public void CriarUsuario_ComDadosValidos_RetornaUsuario()
    {
        var dataNascimento = new DateTimeOffset(1990, 1, 15, 0, 0, 0, TimeSpan.Zero);

        var resultado = Usuario.CriarUsuario("Ana Costa", "ana@email.com", dataNascimento);

        resultado.IsError.Should().BeFalse();
        resultado.Value.NomeCompleto.Should().Be("Ana Costa");
        resultado.Value.Email.Should().Be("ana@email.com");
        resultado.Value.UserName.Should().Be("ana@email.com");
        resultado.Value.Ativo.Should().BeTrue();
        resultado.Value.DataNascimento.Should().Be(dataNascimento);
    }

    [Fact]
    public void CriarUsuario_DataCadastroPreenchidaAutomaticamente()
    {
        var antes = DateTimeOffset.UtcNow;

        var resultado = Usuario.CriarUsuario("Carlos Lima", "carlos@email.com", DateTimeOffset.UtcNow.AddYears(-28));

        resultado.Value.DataCadastro.Should().BeOnOrAfter(antes);
    }

    [Fact]
    public void AdicionarEndereco_AtribuiEnderecoCompleto()
    {
        var usuario = new Usuario("Carlos Lima", "carlos@email.com", DateTimeOffset.UtcNow.AddYears(-28));

        usuario.AdicionarEndereco("Av. Paulista", "SP", "São Paulo", "1000", "01310-100", "Bela Vista", "Apto 42");

        usuario.Endereco.Should().NotBeNull();
        usuario.Endereco!.Logradouro.Should().Be("Av. Paulista");
        usuario.Endereco.UF.Should().Be("SP");
        usuario.Endereco.Cidade.Should().Be("São Paulo");
        usuario.Endereco.Numero.Should().Be("1000");
        usuario.Endereco.CEP.Should().Be("01310-100");
        usuario.Endereco.Complemento.Should().Be("Apto 42");
    }

    [Fact]
    public void AdicionarEndereco_Substituido_MantемNovoEndereco()
    {
        var usuario = new Usuario("Pedro Alves", "pedro@email.com", DateTimeOffset.UtcNow.AddYears(-22));
        usuario.AdicionarEndereco("Rua A", "RJ", "Rio de Janeiro", "10", "20040-020", "Centro", "");

        usuario.AdicionarEndereco("Rua B", "SP", "São Paulo", "20", "01310-100", "Jardins", "");

        usuario.Endereco!.Logradouro.Should().Be("Rua B");
        usuario.Endereco.UF.Should().Be("SP");
    }

    [Fact]
    public void AtualizarFotoPerfil_AtribuiNovaUrl()
    {
        var usuario = new Usuario("Pedro Alves", "pedro@email.com", DateTimeOffset.UtcNow.AddYears(-22));
        var urlFoto = "profiles/pedro-alves-avatar.jpg";

        usuario.AtualizarFotoPerfil(urlFoto);

        usuario.UrlFotoPerfil.Should().Be(urlFoto);
    }

    [Fact]
    public void AtualizarFotoPerfil_Substituida_MantemNovaUrl()
    {
        var usuario = new Usuario("Maria", "maria@email.com", DateTimeOffset.UtcNow.AddYears(-30));
        usuario.AtualizarFotoPerfil("profiles/foto-antiga.jpg");

        usuario.AtualizarFotoPerfil("profiles/foto-nova.jpg");

        usuario.UrlFotoPerfil.Should().Be("profiles/foto-nova.jpg");
    }
}
