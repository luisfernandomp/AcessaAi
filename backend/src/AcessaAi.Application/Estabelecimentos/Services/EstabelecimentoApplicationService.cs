using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Application.Estabelecimentos.Interfaces;
using AcessaAi.Application.Storage.Interfaces;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.Estabelecimentos.Consultas;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.Estabelecimentos.Services
{
    public class EstabelecimentoApplicationService : IEstabelecimentoApplicationService
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public EstabelecimentoApplicationService(
            IEstabelecimentoRepository estabelecimentoRepository,
            IImageStorageService imageStorageService,
            IUnitOfWork unitOfWork)
        {
            _estabelecimentoRepository = estabelecimentoRepository;
            _imageStorageService = imageStorageService; 
            _unitOfWork = unitOfWork;
        }

        private EstabelecimentoResponse ResolverUrls(EstabelecimentoResponse response)
        {
            response.Fotos = [.. response.Fotos
                .Select(f => { f.Url = _imageStorageService.GetPresignedUrl(f.Url); return f; })];
            return response;
        }

        public async Task<ErrorOr<EstabelecimentoResponse>> CriarAsync(
            EstabelecimentoCriarRequest request,
            CancellationToken cancellationToken)
        {
            var geocordenadas = request.Geocordenadas.Adapt<Geocordenadas>();
            var endereco = request.Endereco.Adapt<Endereco>();

            var estabelecimentoResult = Estabelecimento.Criar(request.Nome, request.Tipo, geocordenadas, endereco);

            if (estabelecimentoResult.IsError)
                return estabelecimentoResult.Errors;

            var estabelecimento = estabelecimentoResult.Value;

            if (request.Capa is not null)
            {
                var capaUrl = await _imageStorageService.UploadAsync(
                    request.Capa.Content, request.Capa.FileName, request.Capa.ContentType, cancellationToken);
                estabelecimento.AdicionarImagem(capaUrl, isCapa: true);
            }

            foreach (var foto in request.Fotos)
            {
                var fotoUrl = await _imageStorageService.UploadAsync(
                    foto.Content, foto.FileName, foto.ContentType, cancellationToken);
                estabelecimento.AdicionarImagem(fotoUrl, isCapa: false);
            }

            await _estabelecimentoRepository.AddAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoResponse>());
        }

        public async Task<ErrorOr<EstabelecimentoResponse>> AtualizarAsync(
            int id,
            EstabelecimentoAtualizarRequest request,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(id, cancellationToken);
            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            var geocordenadas = request.Geocordenadas.Adapt<Geocordenadas>();

            estabelecimento.Alterar(request.Nome, geocordenadas, request.Tipo);

            await _estabelecimentoRepository.UpdateAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoResponse>());
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

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoResponse>());
        }

        public async Task<ErrorOr<Success>> SubirImagemAsync(
            int id,
            EstabelecimentoImagemRequest request,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(id, cancellationToken);
            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            var imageUrl = await _imageStorageService.UploadAsync(
                request.Content,
                request.FileName,
                request.ContentType,
                cancellationToken
            );

            estabelecimento.AdicionarImagem(imageUrl, request.IsCapa);

            await _estabelecimentoRepository.UpdateAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success;
        }

        public async Task<ErrorOr<IEnumerable<EstabelecimentoResponse>>> FiltrarAsync(
            EstabelecimentoFiltrarRequest request,
            CancellationToken cancellationToken)
        {
            var consulta = request.Adapt<EstabelecimentoFiltrarConsulta>();

            var estabelecimentos = await _estabelecimentoRepository.FiltrarAsync(consulta, cancellationToken);

            var result = estabelecimentos.Then(list =>
                list.Adapt<List<EstabelecimentoResponse>>()
                    .Select(ResolverUrls)
                    .ToList() as IEnumerable<EstabelecimentoResponse>);

            return result;
        }
    }
}
