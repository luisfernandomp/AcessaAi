
using ErrorOr;

namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities
{
    public static class EstabelecimentoErros
    {
        public static Error NomeObrigatorio =>
            Error.Validation(code: "Estabelecimento.NomeObrigatorio", description: "O nome é obrigatório.");
        
        public static Error GeocordenadasObrigatorio =>
            Error.Validation(code: "Estabelecimento.GeocordenadasObrigatorio", description: "As geocordenadas obrigatórias.");
        
        public static Error MediaEntre0e5 =>
            Error.Validation(code: "Estabelecimento.MediaEntre0e5", description: "A média de estrelas deve ser entre 0 e 5.");
    }
}