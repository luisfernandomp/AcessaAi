using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.Avaliacoes.Interfaces;
using AcessaAi.Application.Dtos;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Interfaces;
using AcessaAi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Application.Avaliacoes.Services
{
    public class AvaliacaoApplicationService : IAvaliacaoApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAvaliacaoService _avaliacaoService;

        public Task<BaseResponse<AvaliacaoResponse>> AtualizarAsync(AvaliacaoUpdateRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<AvaliacaoResponse>> CriarAsync(AvaliacaoCreateRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> ExcluirAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<AvaliacaoResponse>> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
