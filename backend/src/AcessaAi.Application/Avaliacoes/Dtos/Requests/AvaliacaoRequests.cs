namespace AcessaAi.Application.Avaliacoes.Dtos.Requests
{
    public class AvaliacaoCreateRequest
    {
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
        public int UsuarioId { get; set; }
    }

    public class AvaliacaoUpdateRequest
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public ushort Estrelas { get; set; }
    }
}
