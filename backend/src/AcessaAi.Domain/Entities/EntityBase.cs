namespace AcessaAi.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; set; } 
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true; 

        public EntityBase()
        {
            DataCadastro = DateTime.Now;
        }

    }
}
