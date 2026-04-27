using AcessaAi.Domain.Entities;

namespace AcessaAi.Domain.Categorias.Entities
{
    public class Categoria : EntityBase 
    {
        public string Descricao { get; set; }   
        public string UrlIcone { get; set; }
    }
}
