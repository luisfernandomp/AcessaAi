using System;
using AcessaAi.Application.Estabelecimentos.Dtos.Requests;

namespace AcessaAi.API.Extensions;

public static class FormFileExtensions
{
    public static EstabelecimentoImagemRequest ToEstabelecimentoImagemRequest(this IFormFile imagem, bool isCapa = false)
    {
        return new EstabelecimentoImagemRequest
        {
            FileName = imagem.FileName,
            ContentType = imagem.ContentType,
            Content = imagem.OpenReadStream(),
            IsCapa = isCapa
        };
    }

}
