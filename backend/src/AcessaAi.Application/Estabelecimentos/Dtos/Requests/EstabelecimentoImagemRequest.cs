using System;

namespace AcessaAi.Application.Estabelecimentos.Dtos.Requests;

public class EstabelecimentoImagemRequest
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Content { get; set; }
}
