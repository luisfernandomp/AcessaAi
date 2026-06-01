using AcessaAi.Application.Usuarios.Dtos.Requests;
using AcessaAi.Application.Usuarios.Dtos.Responses;
using ErrorOr;

namespace AcessaAi.Application.Usuarios.Interfaces
{
    public interface IUsuarioApplicationService
    {
        Task<ErrorOr<UsuarioResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken);
        Task<ErrorOr<UsuarioResponse>> CadastrarAsync(UsuariosCadastrarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<UsuarioResponse>> AtualizarAsync(int id, UsuariosAtualizarRequest request, CancellationToken cancellationToken);
        Task<ErrorOr<string>> UploadFotoPerfilAsync(int id, Stream conteudo, string nomeArquivo, string contentType, CancellationToken cancellationToken);
    }
}
