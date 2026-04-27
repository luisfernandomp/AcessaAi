namespace AcessaAi.Application.Dtos
{
    public class BaseResponse<T> where T : class
    {
        public T? Resultado { get; set; }
        public bool Sucesso { get; set; }
        public string[] Erros { get; set; } = [];
    }
}
