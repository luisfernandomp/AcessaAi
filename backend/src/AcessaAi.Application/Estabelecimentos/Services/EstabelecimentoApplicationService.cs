using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using ErrorOr;
using Nelibur.ObjectMapper;

namespace AcessaAi.Application.Estabelecimentos.Services
{
    public class EstabelecimentoApplicationService : IEstabelecimentoApplicationService
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;
        private readonly IUnitOfWork _unitOfWork;

            public EstabelecimentoApplicationService(
                IEstabelecimentoRepository estabelecimentoRepository,
            IUnitOfWork unitOfWork)
        {
            _estabelecimentoRepository = estabelecimentoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<EstabelecimentoResponse>> CriarAsync(
            EstabelecimentoCriarRequest request,
            CancellationToken cancellationToken)
        {
            
            var geocordenadas = TinyMapper.Map<Geocordenadas>(request.Geocordenadas);

            var estabelecimentoResult = Estabelecimento.Criar(request.Nome, geocordenadas);

            if (estabelecimentoResult.IsError)
                return estabelecimentoResult.Errors;

            var estabelecimento = estabelecimentoResult.Value;

            await _estabelecimentoRepository.AddAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return TinyMapper.Map<EstabelecimentoResponse>(estabelecimento);
        }

        public async Task<ErrorOr<EstabelecimentoResponse>> AtualizarAsync(
            EstabelecimentoAtualizarRequest request,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(request.Id, cancellationToken);
            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            var geocordenadas = TinyMapper.Map<Geocordenadas>(request.Geocordenadas);

            estabelecimento.Alterar(request.Nome, geocordenadas);

            await _estabelecimentoRepository.UpdateAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return TinyMapper.Map<EstabelecimentoResponse>(estabelecimento);
        }

        public async Task<ErrorOr<Success>> ExcluirAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(id, cancellationToken);
            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            await _estabelecimentoRepository.DeleteAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success;
        }

        public async Task<ErrorOr<EstabelecimentoResponse>> ObterPorIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(id, cancellationToken);

            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            return TinyMapper.Map<EstabelecimentoResponse>(estabelecimento);
        }

    }
}