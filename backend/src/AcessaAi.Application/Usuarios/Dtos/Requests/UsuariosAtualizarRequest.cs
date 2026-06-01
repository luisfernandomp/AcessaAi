using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Usuarios.Dtos.Requests
{
    public class UsuariosAtualizarRequest
    {
        public string Nome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public EnderecoRequest Endereco { get; set; } = null!;
    }
}
