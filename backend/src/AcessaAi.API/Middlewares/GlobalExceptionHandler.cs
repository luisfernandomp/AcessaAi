using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AcessaAi.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
            => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exceção capturada: {Message}", exception.Message);

            var (status, title) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validação falhou"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Não encontrado"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno")
            };

            var problem = new ProblemDetails
            {
                Status = status,
                Title  = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = status;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true; 
        }

    }
}
