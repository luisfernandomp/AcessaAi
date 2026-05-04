
using ErrorOr;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities
{
    public static class EstabelecimentoErros
    {
        public static Error NomeObrigatorio =>
            Error.Validation(code: "Estabelecimento.NomeObrigatorio", description: "O nome é obrigatório.");
        
        public static Error GeocordenadasObrigatorio =>
            Error.Validation(code: "Estabelecimento.GeocordenadasObrigatorio", description: "As geocordenadas obrigatórias.");
        
    }
}