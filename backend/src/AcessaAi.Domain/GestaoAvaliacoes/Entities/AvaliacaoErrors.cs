
using ErrorOr;

namespace AcessaAi.Domain.GestaoAvaliacoes.Entities
{
    public static class AvaliacaoErrors
    {
        public static Error UsuarioIdObrigatorio =>
            Error.Validation(code: "Avaliacao.UsuarioIdObrigatorio", description: "O ID do usuário é obrigatório.");

        public static Error EstabelecimentoIdObrigatorio =>
            Error.Validation(code: "Avaliacao.EstabelecimentoIdObrigatorio", description: "O ID do estabelecimento é obrigatório.");

        public static Error NotaForaDoIntervalo =>
            Error.Validation(code: "Avaliacao.NotaForaDoIntervalo", description: "A nota deve estar entre 1 e 5.");

        public static Error ComentarioObrigatorio =>
            Error.Validation(code: "Avaliacao.ComentarioObrigatorio", description: "O comentário é obrigatório.");

        public static Error ComentarioMuitoLongo =>
            Error.Validation(code: "Avaliacao.ComentarioMuitoLongo", description: "O comentário deve ter no máximo 500 caracteres.");

    }
}
