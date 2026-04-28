namespace AcessaAi.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; set; } 
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true; 

        public EntityBase()
        {
            DataCadastro = DateTimeOffset.UtcNow;
        }

    }
}
