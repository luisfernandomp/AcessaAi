using AcessaAi.Application.Estabelecimentos.Dtos.Requests;
using AcessaAi.Application.Estabelecimentos.Dtos.Responses;
using AcessaAi.Application.Estabelecimentos.Interfaces;
using AcessaAi.Application.Storage.Interfaces;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.Estabelecimentos.Consultas;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Domain.GestaoEstabelecimentos.ValueObjects;
using AcessaAi.Domain.RecursosAcessibilidades.Repositories;
using ErrorOr;
using Mapster;

namespace AcessaAi.Application.Estabelecimentos.Services
{
    public class EstabelecimentoApplicationService : IEstabelecimentoApplicationService
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepository;
        private readonly IRecursoAcessibilidadeRepository _recursoAcessibilidadeRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public EstabelecimentoApplicationService(
            IEstabelecimentoRepository estabelecimentoRepository,
            IRecursoAcessibilidadeRepository recursoAcessibilidadeRepository,
            IImageStorageService imageStorageService,
            IUnitOfWork unitOfWork)
        {
            _estabelecimentoRepository = estabelecimentoRepository;
            _recursoAcessibilidadeRepository = recursoAcessibilidadeRepository;
            _imageStorageService = imageStorageService;
            _unitOfWork = unitOfWork;
        }

        private EstabelecimentoListarResponse ResolverUrls(EstabelecimentoListarResponse response)
        {
            response.Fotos = [.. response.Fotos
                .Select(f => { f.Url = _imageStorageService.GetPresignedUrl(f.Url); return f; })];
            return response;
        }

        public async Task<ErrorOr<EstabelecimentoListarResponse>> CriarAsync(
            EstabelecimentoCriarRequest request,
            CancellationToken cancellationToken)
        {
            var geocordenadas = request.Geocordenadas.Adapt<Geocordenadas>();
            var endereco = request.Endereco.Adapt<Endereco>();

            var estabelecimentoResult = Estabelecimento.Criar(request.Nome, request.Tipo, geocordenadas, endereco);

            if (estabelecimentoResult.IsError)
                return estabelecimentoResult.Errors;

            var estabelecimento = estabelecimentoResult.Value;

            if (request.CapaChave is not null)
                estabelecimento.AdicionarImagem(request.CapaChave, isCapa: true);

            foreach (var chave in request.FotosChaves)
                estabelecimento.AdicionarImagem(chave, isCapa: false);

            foreach (var id in request.RecursosAcessibilidadesIds ?? [])
            {
                var recurso = await _recursoAcessibilidadeRepository.ObterPorIdAsync(id, cancellationToken);
                if (recurso is not null)
                    estabelecimento.AdicionarRecursoAcessibilidade(recurso);
            }

            await _estabelecimentoRepository.AddAsync(estabelecimento, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoListarResponse>());
        }

        public async Task<ErrorOr<EstabelecimentoListarResponse>> AtualizarAsync(
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

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoListarResponse>());
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

        public async Task<ErrorOr<EstabelecimentoListarResponse>> ObterPorIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepository.ObterPorIdAsync(id, cancellationToken);

            if (estabelecimento is null)
                return Error.NotFound("Estabelecimento.NaoEncontrado", "Estabelecimento não encontrado.");

            return ResolverUrls(estabelecimento.Adapt<EstabelecimentoListarResponse>());
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

        public async Task<ErrorOr<IEnumerable<EstabelecimentoListarResponse>>> FiltrarAsync(
            EstabelecimentoFiltrarRequest request,
            CancellationToken cancellationToken)
        {
            var consulta = new EstabelecimentoFiltrarConsulta
            {
                Nome = request.Nome,
                Tipo = request.Tipo,
                DistanciaMaxima = request.DistanciaMaxima,
                Latitude = request.LatitudeResolvida,
                Longitude = request.LongitudeResolvida,
                EnderecoConsulta = request.EnderecoRequest?.Adapt<EnderecoConsulta>(),
                RecursosAcessabilidadeIds = request.RecursosAcessabilidadeIds,
            };

            var estabelecimentos = await _estabelecimentoRepository.FiltrarAsync(consulta, cancellationToken);

            var result = estabelecimentos.Then(list =>
            {
                var responses = list.Adapt<List<EstabelecimentoListarResponse>>()
                    .Select(ResolverUrls)
                    .ToList();

                if (consulta.Latitude.HasValue && consulta.Longitude.HasValue)
                {
                    var lat = consulta.Latitude.Value;
                    var lng = consulta.Longitude.Value;
                    foreach (var r in responses)
                    {
                        var dLat = (r.Geocordenadas.Latitude - lat) * Math.PI / 180.0;
                        var dLng = (r.Geocordenadas.Longitude - lng) * Math.PI / 180.0;
                        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                              + Math.Cos(lat * Math.PI / 180.0)
                              * Math.Cos(r.Geocordenadas.Latitude * Math.PI / 180.0)
                              * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                        r.DistanciaKm = 6371.0 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    }
                }

                return responses as IEnumerable<EstabelecimentoListarResponse>;
            });

            return result;
        }
    }
}
