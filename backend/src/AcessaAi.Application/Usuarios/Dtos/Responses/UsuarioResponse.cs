using AcessaAi.Application.Dtos;

namespace AcessaAi.Application.Usuarios.Dtos.Responses
{
    public record UsuarioResponse
    {
        public string Nome { get; set; }
        public EnderecoResponse Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
