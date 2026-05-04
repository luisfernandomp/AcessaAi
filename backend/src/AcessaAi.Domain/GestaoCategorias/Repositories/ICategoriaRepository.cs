using System;
using AcessaAi.Domain.Categorias.Entities;
using AcessaAi.Domain.Common;
using AcessaAi.Domain.GestaoCategorias.Consultas;

namespace AcessaAi.Domain.GestaoCategorias.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<List<CategoriaListarConsulta>> ListarAtivasAsync(CancellationToken cancellationToken);  
}
