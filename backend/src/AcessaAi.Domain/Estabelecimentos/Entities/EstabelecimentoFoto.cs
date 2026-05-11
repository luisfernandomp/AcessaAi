namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities;

public class EstabelecimentoFoto
{
    public int Id { get; private set; }
    public string Url { get; private set; } = null!;
    public int EstabelecimentoId { get; private set; }

    private EstabelecimentoFoto() { }

    public EstabelecimentoFoto(string url)
    {
        Url = url;
    }
}
