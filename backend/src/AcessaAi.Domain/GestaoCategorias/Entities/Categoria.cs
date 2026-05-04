using AcessaAi.Domain.Common;

namespace AcessaAi.Domain.Categorias.Entities
{
    public class Categoria : EntityBase 
    {
        public string Nome {get;set;}
        public string Descricao { get; set; }   
        public string Icone { get; set; }
    }
}
