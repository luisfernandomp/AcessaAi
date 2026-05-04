using AcessaAi.Application.Avaliacoes.Dtos.Requests;
using AcessaAi.Application.Avaliacoes.Dtos.Responses;
using AcessaAi.Application.Avaliacoes.Interfaces;
using AcessaAi.Application.Estabelecimentos.Services;
using AcessaAi.Domain.Autenticacao.Entities;
using AcessaAi.Domain.Avaliacoes.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoAvaliacoes.Comandos;
using AcessaAi.Domain.GestaoAvaliacoes.Repositories;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;

namespace AcessaAi.Application.Avaliacoes.Services
{
    public class AvaliacaoApplicationService : IAvaliacaoApplicationService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEstabelecimentoApplicationService _estabelecimentoApplicationService;

        public AvaliacaoApplicationService(
            IAvaliacaoRepository avaliacaoRepository,
            IUnitOfWork unitOfWork,
            UserManager<Usuario> userManager,
            IEstabelecimentoApplicationService estabelecimentoApplicationService)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _estabelecimentoApplicationService = estabelecimentoApplicationService;
        }

        public async Task<ErrorOr<AvaliacaoResponse>> CriarAsync(
            AvaliacaoCreateRequest request,
            CancellationToken cancellationToken)
        {

            var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UsuarioId, cancellationToken);
            var estabelecimento = await _estabelecimentoApplicationService.ObterPorIdAsync(request.EstabelecimentoId, cancellationToken);

            var comando = TinyMapper.Map<AvaliacaoCriarComando>(request);

            //comando.Estabelecimento = estabelecimento.Value;
            comando.Usuario = usuario;

            var avaliacaoResult = Avaliacao.Criar(
                comando.Comentario,
                comando.Estrelas,
                comando.Usuario,
                comando.Estabelecimento);

            if (avaliacaoResult.IsError)
                return avaliacaoResult.Errors;

            var avaliacao = avaliacaoResult.Value;

            await _avaliacaoRepository.AddAsync(avaliacao, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return TinyMapper.Map<AvaliacaoResponse>(avaliacao);
        }

        public async Task<ErrorOr<AvaliacaoResponse>> AtualizarAsync(
            AvaliacaoUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var comando = TinyMapper.Map<AvaliacaoAtualizarComando>(request);

            var avaliacao = await _avaliacaoRepository.ObterPorIdAsync(comando.Id, cancellationToken);
            if (avaliacao is null)
                return Error.NotFound("Avaliacao.NaoEncontrada", "Avaliação não encontrada.");

            avaliacao.Alterar(comando.Comentario, comando.Estrelas);

            await _avaliacaoRepository.UpdateAsync(avaliacao, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return TinyMapper.Map<AvaliacaoResponse>(avaliacao);
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

            return TinyMapper.Map<AvaliacaoResponse>(avaliacao);
        }
    }
}