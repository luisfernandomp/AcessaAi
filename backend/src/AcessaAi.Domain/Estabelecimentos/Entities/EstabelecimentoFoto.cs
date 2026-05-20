namespace AcessaAi.Domain.GestaoEstabelecimentos.Entities;

public class EstabelecimentoFoto
{
    public int Id { get; private set; }
    public string Url { get; private set; } = null!;
    public bool IsCapa { get; private set; }
    public int EstabelecimentoId { get; private set; }

    private EstabelecimentoFoto() { }

    public EstabelecimentoFoto(string url, bool isCapa = false)
    {
        Url = url;
        IsCapa = isCapa;
    }
}
