using AcessaAi.Domain.Estabelecimentos.Consultas;
using AcessaAi.Domain.GestaoEstabelecimentos.Entities;
using AcessaAi.Domain.GestaoEstabelecimentos.Repositories;
using AcessaAi.Infrastructure.Identity;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace AcessaAi.Infrastructure.Repositories
{
    public class EstabelecimentoRepository : Repository<Estabelecimento>, IEstabelecimentoRepository
    {
        public EstabelecimentoRepository(AcessaAiDbContext context) : base(context)
        {
        }

        public override async Task<Estabelecimento> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet
            .Include(x => x.Avaliacoes)
                .ThenInclude(x => x.Usuario)
            .Include(x => x.Fotos)
            .Include(x => x.RecursosAcessibilidade)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExisteEstabelecimentoAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Estabelecimentos.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<ErrorOr<List<Estabelecimento>>> FiltrarAsync(EstabelecimentoFiltrarConsulta consulta, CancellationToken cancellationToken)
        {
            var query = _context.Estabelecimentos
                .Include(e => e.RecursosAcessibilidade)
                .Include(e => e.Fotos)
                .Include(e => e.Avaliacoes)
                .AsQueryable();

            if (!string.IsNullOrEmpty(consulta.Nome))
            {
                query = query.Where(e => e.Nome.Contains(consulta.Nome));
            }

            if (consulta.Tipo.HasValue)
            {
                query = query.Where(e => e.Tipo == consulta.Tipo.Value);
            }

            if (consulta.RecursosAcessabilidadeIds != null && consulta.RecursosAcessabilidadeIds.Any())
            {
                query = query.Where(e => e.RecursosAcessibilidade.Any(r => consulta.RecursosAcessabilidadeIds.Contains(r.Id)));
            }

            if (consulta.Latitude.HasValue && consulta.Longitude.HasValue && consulta.DistanciaMaxima.HasValue)
            {
                var lat = consulta.Latitude.Value;
                var lng = consulta.Longitude.Value;
                var distKm = consulta.DistanciaMaxima.Value;

                var latDelta = distKm / 111.0;
                var lngDelta = distKm / (111.0 * Math.Cos(lat * Math.PI / 180.0));

                query = query.Where(e =>
                    e.Geolocalizacao.Latitude >= lat - latDelta &&
                    e.Geolocalizacao.Latitude <= lat + latDelta &&
                    e.Geolocalizacao.Longitude >= lng - lngDelta &&
                    e.Geolocalizacao.Longitude <= lng + lngDelta);

                var candidatos = await query.ToListAsync(cancellationToken);

                var teste = candidatos.Select(e =>
                {
                    var dLat = (e.Geolocalizacao.Latitude - lat) * Math.PI / 180.0;
                    var dLng = (e.Geolocalizacao.Longitude - lng) * Math.PI / 180.0;
                    var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                            Math.Cos(lat * Math.PI / 180.0) * Math.Cos(e.Geolocalizacao.Latitude * Math.PI / 180.0) *
                            Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                    var distancia = 6371.0 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    return distancia;
                }).ToList();


                return candidatos.Where(e =>
                {
                    var dLat = (e.Geolocalizacao.Latitude - lat) * Math.PI / 180.0;
                    var dLng = (e.Geolocalizacao.Longitude - lng) * Math.PI / 180.0;
                    var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                            Math.Cos(lat * Math.PI / 180.0) * Math.Cos(e.Geolocalizacao.Latitude * Math.PI / 180.0) *
                            Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                    var distancia = 6371.0 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    return distancia <= distKm;
                }).ToList();
            }

            return await query.ToListAsync(cancellationToken);
        }

    }
}
