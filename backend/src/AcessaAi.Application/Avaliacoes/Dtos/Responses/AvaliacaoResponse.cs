namespace AcessaAi.Application.Avaliacoes.Dtos.Responses
{
    public class AvaliacaoResponse
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}
