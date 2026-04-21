using AcessaAi.Application.Autenticacao.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcessaAi.Application.Autenticacao.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
