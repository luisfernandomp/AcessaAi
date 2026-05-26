using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.Avaliacoes.Interfaces;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Repositories;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Domain.GestaoUsuarios.Repositories;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.Avaliacoes.Services
{
    public class AvaliacaoApplicationService : IAvaliacaoApplicationService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AvaliacaoApplicationService(
            IAvaliacaoRepository avaliacaoRepository,
            IEstabelecimentoRepository estabelecimentoRepository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _estabelecimentoRepository = estabelecimentoRepository;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<AvaliacaoResponse>> CriarAsync(
            AvaliacaoCreateRequest request,
            CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId, cancellationToken);
            if (usuario is null)
                return Error.NotFound("Usuario.NaoEncontrado", "Usuário não encontrado.");

            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(request.EstabelecimentoId, cancellationToken);
            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            var avaliacaoResult = Avaliacao.Criar(request.Comentario, request.Estrelas, usuario, estabelecimento);


            if (avaliacaoResult.IsError)
                return avaliacaoResult.Errors;

            var avaliacao = avaliacaoResult.Value;
            estabelecimento.AdicionarAvaliacao(avaliacao);

            await _avaliacaoRepository.AddAsync(avaliacao, cancellationToken);
            await _estabelecimentoRepository.UpdateAsync(estabelecimento, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return avaliacao.Adapt<AvaliacaoResponse>();
        }

        public async Task<ErrorOr<AvaliacaoResponse>> AtualizarAsync(
            AvaliacaoUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacaoRepository.ObterPorIdAsync(request.Id, cancellationToken);
            if (avaliacao is null)
                return Error.NotFound("Avaliacao.NaoEncontrada", "Avaliação não encontrada.");

            avaliacao.Alterar(request.Comentario, request.Estrelas);

            await _avaliacaoRepository.UpdateAsync(avaliacao, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return avaliacao.Adapt<AvaliacaoResponse>();
        }

        public async Task<ErrorOr<Success>> ExcluirAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacaoRepository.ObterPorIdAsync(id, cancellationToken);
            if (avaliacao is null)
                return Error.NotFound("Avaliacao.NaoEncontrada", "Avaliação não encontrada.");

            await _avaliacaoRepository.DeleteAsync(avaliacao, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success;
        }

        public async Task<ErrorOr<AvaliacaoResponse>> ObterPorIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacaoRepository.ObterPorIdAsync(id, cancellationToken);

            if (avaliacao is null)
                return Error.NotFound("Avaliacao.NaoEncontrada", "Avaliação não encontrada.");

            return avaliacao.Adapt<AvaliacaoResponse>();
        }
    }
}
