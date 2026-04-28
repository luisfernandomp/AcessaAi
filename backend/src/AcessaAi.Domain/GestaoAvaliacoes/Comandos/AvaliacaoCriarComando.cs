namespace AcessaAi.Domain.GestaoAvaliacoes.Comandos
{
    public class AvaliacaoCriarComando
    {
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
        public int UsuarioId { get; set; }
        public int EstabelecimentoId { get; set; }
    }
}
