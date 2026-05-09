using AcessaAi.Domain.Common;

namespace AcessaAi.Domain.RecursosAcessibilidades.Entities
{
    public class RecursoAcessibilidade : EntityBase 
    {
        public string Nome {get;set;}
        public string Descricao { get; set; }   
        public string Icone { get; set; }
    }
}
