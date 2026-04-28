namespace AcessaAi.Application.Dtos
{
    public class BaseResponse
    {
        public bool Sucesso { get; set; }
        public string[] Erros { get; set; } = [];

        public static BaseResponse Ok()
            => new() { Sucesso = true };    

        public static BaseResponse Falha(params string[] erros)
            => new() { Sucesso = false, Erros = erros };
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T? Resultado { get; set; }

        public static BaseResponse<T> Ok(T resultado)
            => new() { Sucesso = true, Resultado = resultado };

        public static new BaseResponse<T> Falha(params string[] erros)
            => new() { Sucesso = false, Erros = erros };
    }
}
