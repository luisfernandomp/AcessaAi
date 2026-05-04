using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AcessaAi.API.Extensions
{
    public static class ErrorOrExtensions
    {
        public static IActionResult ToActionResult<T>(
            this ErrorOr<T> result,
            Func<T, IActionResult> onSuccess)
        {
            if(!result.IsError)
                return onSuccess(result.Value);

            var firstError = result.Errors[0];

            return firstError.Type switch
            {
                ErrorType.Validation => 
                    new BadRequestObjectResult(
                        BuildProblem(result.Errors, HttpStatusCode.BadRequest, "Erros de validação")),
                ErrorType.NotFound =>
                    new NotFoundObjectResult(
                        BuildProblem(result.Errors, HttpStatusCode.NotFound, "Recurso não encontrado ")) ,
                ErrorType.Conflict =>
                    new ConflictObjectResult(
                        BuildProblem(result.Errors, HttpStatusCode.Conflict, " Conflito ")),
                ErrorType.Unauthorized =>
                    new UnauthorizedObjectResult(
                        BuildProblem(result.Errors, HttpStatusCode.Unauthorized, "Não autorizado ")),
                _ => new ObjectResult(
                        BuildProblem(result.Errors, HttpStatusCode.InternalServerError, " Erro interno "))
            };
        }

        private static object BuildProblem(List<Error> errors, HttpStatusCode status, string title)
        {
            return new
            {
                status = status.ToString(),
                title,
                errors = errors.Select(e => new
                {
                    code = e.Code,
                    description = e.Description
                })
            };
        }
    }
}
