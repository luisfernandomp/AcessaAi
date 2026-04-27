using AcessaAi.Application.Dtos.Requests;

namespace AcessaAi.Application.Usuarios.Dtos.Requests
{
    public class UsuariosCadastrarRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataNascimento { get; set; }
        public EnderecoRequest Endereco { get; set; }
    }
}
